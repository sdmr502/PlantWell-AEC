using System;
using Windows.ApplicationModel.Background;

using System.IO;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Threading;

using Windows.Devices.Gpio;
using Microsoft.Extensions.Configuration;

using Sensors.Dht;
using PlantWellBgClient.Models;
using PlantWellBgClient.Logic;


using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using PlantWellBgClient.DAL;

namespace PlantWellBgClient
{
    public sealed class StartupTask : IBackgroundTask
    {

        private static IConfiguration _iconfiguration;

        /**
         * Define inputs 
         */

        private GpioPin INTAKE_DHT_SENSOR = GpioController.GetDefault().OpenPin(22, GpioSharingMode.Exclusive);
        private DHTData intakeDHTData;

        private GpioPin OUTTAKE_DHT_SENSOR = GpioController.GetDefault().OpenPin(25, GpioSharingMode.Exclusive);
        private DHTData outtakeDHTData;

        private GpioPin MAIN_DHT_SENSOR = GpioController.GetDefault().OpenPin(16, GpioSharingMode.Exclusive);
        private DHTData mainDHTData;

        /**
         * Define Outputs 
         */

        private GpioPin Lighting = GpioController.GetDefault().OpenPin(17);

        /**
         * Side Right = Outtake 
         * Side Left = Intake
         */

        private GpioPin OuttakeExitFan = GpioController.GetDefault().OpenPin(24);

        private GpioPin OuttakeRecircFan = GpioController.GetDefault().OpenPin(26);

        private GpioPin OuttakeEntFan = GpioController.GetDefault().OpenPin(19);


        private GpioPin IntakeEntFan = GpioController.GetDefault().OpenPin(23);

        private GpioPin IntakeExitBottomFan = GpioController.GetDefault().OpenPin(27);

        private GpioPin IntakeExitTopFan = GpioController.GetDefault().OpenPin(13);

        private GpioPin LightCoolingFan = GpioController.GetDefault().OpenPin(6);


        private GpioPin SparePin = GpioController.GetDefault().OpenPin(5);



        private SqlConnection myConnection = null;

        private string _connectionString;
        
        private SqlCommand cmd;
        
        EnvironmentData EnvData = new EnvironmentData();


        public void Run(IBackgroundTaskInstance taskInstance)
        {
            //_iconfiguration.GetSection("HardwareSpecifications");
            //_iconfiguration.GetSection("SoftwareSpecifications");


            GpioPin[] PollSensors = { MAIN_DHT_SENSOR, OUTTAKE_DHT_SENSOR, INTAKE_DHT_SENSOR };
            string[] PollSensorNames = {"Main", "Outtake", "Intake" };

            //Get App Settings
            GetAppSettingsFile();

            //Initiate and Open Datebase Connection 
            startDbConnection();

            //Initiate Outputs. Set to Low and check it's wrote
            //InitiateOutput(Lighting);
            //InitiateOutput(OuttakeExitFan);
            //InitiateOutput(OuttakeRecircFan);
            //InitiateOutput(OuttakeEntFan)
            //InitiateOutput(IntakeEntFan);
            //InitiateOutput(IntakeExitBottomFan);
            //InitiateOutput(IntakeExitTopFan);

            //InitiateOutput(LightCoolingFan);

            //InitiateOutput(SparePin);

            // Prepare the environment with default values
            EnvData.targetTemp = 24 ;
            EnvData.targetHum = 55;

            float TEMPERATURE_THRESHOLD_PERCENTAGE = 15;
            float HUMIDITY_THRESHOLD_PERCENTAGE = 5;

            EnvData.thresholdTemp = Controller.GetThreshold(TEMPERATURE_THRESHOLD_PERCENTAGE, EnvData.targetTemp);
            EnvData.thresholdHum = Controller.GetThreshold(HUMIDITY_THRESHOLD_PERCENTAGE, EnvData.targetHum);

            EnvData.tempMin = Controller.ReturnThreshold(EnvData.thresholdTemp, EnvData.targetTemp, "-");
            EnvData.tempMax = Controller.ReturnThreshold(EnvData.thresholdTemp, EnvData.targetTemp, "+");

            EnvData.humidityMin = Controller.ReturnThreshold(EnvData.thresholdHum, EnvData.targetHum, "-");
            EnvData.humidityMax = Controller.ReturnThreshold(EnvData.thresholdHum, EnvData.targetHum, "+");

            SetRelay(OuttakeExitFan,true);
            SetRelay(OuttakeRecircFan, true);
            SetRelay(OuttakeEntFan, true);
            SetRelay(IntakeExitTopFan, true);
            SetRelay(IntakeEntFan, true);
            SetRelay(IntakeExitBottomFan, true);

            //Infinate Loop
            while (true)
            {
                //Check Db Connection is Open and Continue
                if (myConnection.State == ConnectionState.Open)
                {


                    //Check if main light needs to be on

                    if (IsTimeOfDayBetween(DateTime.Now, new TimeSpan(23, 00, 0), new TimeSpan(04, 00, 0)))
                    {
                        SetRelay(Lighting, true);
                    }
                    else
                    {
                        SetRelay(Lighting, false);
                    }

                    for (int i = 0; i < PollSensors.Length; i++)
                    {
                       
                        Debug.WriteLine(string.Format("Querying - {0} DHT Sensor", PollSensorNames[i]));
                        MainPollDHT(PollSensors[i], PollSensorNames[i]);
                        
                        
                        //SensorReadingsDAL.Load5MinuteAverage(PollSensorNames[i], myConnection);

                        System.Threading.Thread.Sleep(500);
                    }

                    //Make a query to get the average for the last 5 minutes on each column applicable 

                    if (mainDHTData != null && intakeDHTData != null && outtakeDHTData != null)
                    {
                        Debug.WriteLine("Processing... \n");

                        EnvData.Lighting = (float)getHwStatus(Lighting);
                        EnvData.OuttakeExitFan = (float)getHwStatus(OuttakeExitFan);
                        EnvData.OuttakeRecircFan = (float)getHwStatus(OuttakeRecircFan);
                        EnvData.OuttakeEntFan = (float)getHwStatus(OuttakeEntFan);

                        EnvData.IntakeEntFan = (float)getHwStatus(IntakeEntFan);
                        EnvData.IntakeExitBottomFan = (float)getHwStatus(IntakeExitBottomFan);
                        EnvData.IntakeExitTopFan = (float)getHwStatus(IntakeExitTopFan);


                        EnvData.MainTemp = (float)(double)mainDHTData.Temp;
                        EnvData.MainHum = (float)(double)mainDHTData.Hum;
                        EnvData.IntakeTemp = (float)(double)intakeDHTData.Temp;
                        EnvData.IntakeHum = (float)(double)intakeDHTData.Hum;
                        EnvData.OuttakeTemp = (float)(double)outtakeDHTData.Temp;
                        EnvData.OuttakeHum = (float)(double)outtakeDHTData.Hum;





                        bool checkTemperatureThreshold = Controller.WithinThreshold(EnvData.thresholdTemp, EnvData.targetTemp, EnvData.MainTemp);
                        bool checkHumidityThreshold = Controller.WithinThreshold(EnvData.thresholdHum, EnvData.targetHum, EnvData.MainHum);


                        EnvData.Option = Convert.ToInt32(string.Format("{0}{1}{2}{3}{4}{5}",
                            Controller.Balancer(EnvData.MainTemp, EnvData.tempMin, EnvData.tempMax),
                            Controller.Balancer(EnvData.MainHum, EnvData.humidityMin, EnvData.humidityMax),
                            Controller.Balancer(EnvData.IntakeTemp, EnvData.tempMin, EnvData.tempMax),
                            Controller.Balancer(EnvData.IntakeHum, EnvData.humidityMin, EnvData.humidityMax),
                            Controller.Balancer(EnvData.OuttakeTemp, EnvData.tempMin, EnvData.tempMax),
                            Controller.Balancer(EnvData.OuttakeHum, EnvData.humidityMin, EnvData.humidityMax)
                         ));
                        Debug.WriteLine(EnvData.Option);



                        if (checkTemperatureThreshold && checkHumidityThreshold)
                        {
                            EnvData.Version = "NoPrediction";
                            Debug.WriteLine("Relax Everything is Good :)");
                            SetRelay(OuttakeRecircFan, true);
                            SetRelay(IntakeEntFan, false);
                            SetRelay(IntakeExitBottomFan, false);
                            SetRelay(IntakeExitTopFan, false);


                            if (EnvData.OuttakeTemp > EnvData.tempMax)
                            {
                                SetRelay(OuttakeExitFan, true);

                            }
                            else
                            {
                                SetRelay(OuttakeExitFan, false);

                            }
                        }
                        else if (checkHumidityThreshold == false && checkTemperatureThreshold)
                        {
                            EnvData.Version = "HumMLModel";
                            //Controller.correctBalance(EnvData, "HumPredict");
                            Debug.WriteLine("Humidity Correction Required");
                            SetRelay(OuttakeExitFan, true);
                            SetRelay(OuttakeRecircFan, false);
                            SetRelay(IntakeEntFan, false);
                            SetRelay(IntakeExitBottomFan, true);
                            SetRelay(IntakeExitTopFan, true);


                        }
                        else if (checkHumidityThreshold && checkTemperatureThreshold == false)
                        {
                            EnvData.Version = "MLModel";
                            //Controller.correctBalance(EnvData, "TempPredict");
                            Debug.WriteLine("Temperature Correction Required");
                            SetRelay(OuttakeExitFan, true);
                            SetRelay(OuttakeRecircFan, false);
                            SetRelay(IntakeEntFan, true);
                            SetRelay(IntakeExitBottomFan, true);
                            SetRelay(IntakeExitTopFan, true);

                        }
                        else
                        {
                            Debug.WriteLine("Enviroment Unstable");
                            EnvData.Version = "EnvMLModel";
                            //Controller.correctBalance(EnvData, "TempPredict");
                            SetRelay(OuttakeExitFan, true);
                            SetRelay(OuttakeRecircFan, false);
                            SetRelay(IntakeEntFan, true);
                            SetRelay(IntakeExitBottomFan, true);
                        }
                        string cmdString = "INSERT INTO [dbo].[environment] ([timestamp],[profileId],[mainTemp],[mainHum],[intakeTemp],[intakeHum],[outtakeTemp],[outtakeHum],  [Lighting], [OuttakeExitFan],[OuttakeRecircFan],[IntakeEntFan],[IntakeExitBottomFan],[IntakeExitTopFan],[OuttakeEntFan],[version]) ";
                        cmdString += "VALUES('" + DateTime.Now + "','1', '" + EnvData.MainTemp + "', '" + EnvData.MainHum + "', '" + EnvData.IntakeTemp + "', '" + EnvData.IntakeHum + "', '" + EnvData.OuttakeTemp + "', '" + EnvData.OuttakeHum + "', '" + EnvData.Lighting + "', '" + EnvData.OuttakeExitFan + "', '" + EnvData.OuttakeRecircFan + "', '" + EnvData.IntakeEntFan + "', '" + EnvData.IntakeExitBottomFan + "', '" + EnvData.IntakeExitTopFan + "', '" + EnvData.OuttakeEntFan + "', '" + EnvData.Version + "'); ";



                        cmd = new SqlCommand(cmdString, myConnection);

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException e)
                        {
                            Debug.WriteLine(string.Format("Failed - {0}", e.Message.ToString()));

                        }
                    }
                }
                else if (myConnection.State == ConnectionState.Closed)
                {
                    startDbConnection();
                    Debug.WriteLine("Waiting 5 seconds for the database connection to be established");
                    System.Threading.Thread.Sleep(5000);
                }
                System.Threading.Thread.Sleep(300);
            }

        }

        private async void MainPollDHT(GpioPin gpioPin, string Sensor)
        {
            Dht22 dht22 = new Dht22(gpioPin, GpioPinDriveMode.Input);
            DhtReading reading = await dht22.GetReadingAsync().AsTask();
            Debug.WriteLine(string.Format("{0} DHT Valid {1}", Sensor, reading.IsValid));

            if (reading.IsValid)
            {

                Debug.WriteLine(string.Format("{0} DHT Reading:", Sensor));
                Debug.WriteLine(reading.Temperature);
                Debug.WriteLine(reading.Humidity);
                if (Sensor == "Intake")
                {
                    intakeDHTData = (new DHTData()
                    {
                        Temp = reading.Temperature,
                        Hum = reading.Humidity
                    });
                }
                else if (Sensor == "Outtake")
                {
                    outtakeDHTData = (new DHTData()
                    {
                        Temp = reading.Temperature,
                        Hum = reading.Humidity
                    });
                }
                else if (Sensor == "Main")
                {
                    mainDHTData = (new DHTData()
                    {
                        Temp = reading.Temperature,
                        Hum = reading.Humidity
                    });

                }

            System.Threading.Thread.Sleep(1000);
            string cmdString = "INSERT INTO [dbo].[sensorReadings] ([timestamp],[sensor],[temp], [hum], [retryCount]) ";
            cmdString += "VALUES('" + DateTime.Now + "','" + Sensor + "', '" + reading.Temperature + "', '" + reading.Humidity + "', '" + reading.RetryCount + "');";
            cmd = new SqlCommand(cmdString, myConnection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
          
                Debug.WriteLine(string.Format("Failed SQL- {0}", e.Message.ToString()));

            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine(string.Format("Failed Bullshit- {0}", e.Message.ToString()));

            }
            catch (InvalidOperationException e)
            {
                Debug.WriteLine(string.Format("Failed Bullshit- {0}", e.Message.ToString()));

            }
        }
            else
            {

               //Error something here

            }
        }

        static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _iconfiguration = builder.Build();
        }

        private void SetRelay(GpioPin gpioPin, bool position)
        {
            System.Threading.Thread.Sleep(750);
            if (position)
            {
                gpioPin.Write(GpioPinValue.High);
                gpioPin.SetDriveMode(GpioPinDriveMode.Output);
            }
            else
            {
                gpioPin.Write(GpioPinValue.Low);
                gpioPin.SetDriveMode(GpioPinDriveMode.Output);
            }
        }

        private GpioPinValue getHwStatus(GpioPin gpioPin)
        {
            return gpioPin.Read();
        }

        private bool InitiateOutput(GpioPin gpioPin)
        {
            System.Threading.Thread.Sleep(750);
            Debug.WriteLine("Initiate Output Pin");
            gpioPin.Write(GpioPinValue.Low);
            gpioPin.SetDriveMode(GpioPinDriveMode.Output);
            if (gpioPin.Read() == GpioPinValue.Low)
            {
                Debug.WriteLine("Initiated Output Pin");
                return true;
            }
            Debug.WriteLine("Failed to Initiate Output Pin");
            return false;
        }

        private void startDbConnection()
        {
            _connectionString = _iconfiguration.GetConnectionString("Default");
            if (myConnection == null || myConnection.State == ConnectionState.Closed)
            {
                myConnection = new SqlConnection(_connectionString);
                myConnection.Open();
            }

        }

        private static bool IsTimeOfDayBetween(DateTime time,
                              TimeSpan startTime, TimeSpan endTime)
        {
            if (endTime == startTime)
            {
                return true;
            }
            else if (endTime < startTime)
            {
                return time.TimeOfDay <= endTime ||
                    time.TimeOfDay >= startTime;
            }
            else
            {
                return time.TimeOfDay >= startTime &&
                    time.TimeOfDay <= endTime;
            }

        }

    }
}