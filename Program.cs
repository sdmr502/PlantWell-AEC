using Microsoft.Extensions.Configuration;
using PlantWell.DAL;
using PlantWell.Exceptions;
using PlantWell.Services;
using PlantWell.Types;
using System;
using System.IO;
using Unosquare.RaspberryIO;

namespace PlantWell
{
    class Program
    {
        private static IConfiguration _iconfiguration;

        static void Main(string[] args)
        { 
            GetAppSettingsFile();

            var driver = new PiDriver();
            
            try
            {
                var intakeDHT = new DHT(driver.INTAKE_DHT_SENSOR, DHTSensorTypes.DHT22);
                var mainDHT = new DHT(driver.MAIN_DHT_SENSOR, DHTSensorTypes.DHT22);
                var outtakeDHT = new DHT(driver.INTAKE_DHT_SENSOR, DHTSensorTypes.DHT22);

                while (true)
                {
                    try
                    {
                        var d = intakeDHT.ReadData();
                        var d1 = mainDHT.ReadData();
                        var d2 = outtakeDHT.ReadData();

                        Console.WriteLine(DateTime.UtcNow);
                        Console.WriteLine("Intake Temp: " + d.TempCelcius);
                        Console.WriteLine("Intake Hum: " + d.Humidity);

                        Console.WriteLine(DateTime.UtcNow);
                        Console.WriteLine("Main Temp: " + d1.TempCelcius);
                        Console.WriteLine("Main Hum: " + d1.Humidity);

                        Console.WriteLine(DateTime.UtcNow);
                        Console.WriteLine("Outtake Temp: " + d2.TempCelcius);
                        Console.WriteLine("Outtake Hum: " + d2.Humidity);
                    }
                    catch (DHTException)
                    {
                    }
                    Pi.Timing.SleepMilliseconds(10000);
                }
            }
            catch (Exception)
            {
            }
     
        }

        static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _iconfiguration = builder.Build();
        }

        static void GetEnvironment()
        {
            var environmentDAL = new EnvironmentDAL(_iconfiguration);
            var listEnvironmentModel = environmentDAL.GetList("SELECT ");
            listEnvironmentModel.ForEach(item =>
            {
                Console.WriteLine(item);
            });

            Console.WriteLine("Press any key to stop.");

            Console.ReadKey();
        }
    }
}
