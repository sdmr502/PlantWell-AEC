using System;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace PlantWell.Services
{
    class PiDriver
    {
        /**
         * Define inputs 
         */
        public GpioPin MOISTURE_SENSOR_1;
        public GpioPin INTAKE_DHT_SENSOR;
        public GpioPin MAIN_DHT_SENSOR;
        public GpioPin OUTTAKE_DHT_SENSOR;


        /**
         * Define Outputs 
         */
        public GpioPin MAIN_LIGHTING;
        public GpioPin SIDERIGHT_FAN;
        public GpioPin SIDELEFT_FAN;

        /**
         * Define Controllers
         */
        //public GpioPin PWMModule;

        public PiDriver() 
        {
            Pi.Init<BootstrapWiringPi>();

            /** Inputs **/
            MOISTURE_SENSOR_1 = (GpioPin)Pi.Gpio[BcmPin.Gpio21];
            MOISTURE_SENSOR_1.PinMode = GpioPinDriveMode.Input;
            //MOISTURE_SENSOR_1.RegisterInterruptCallback(EdgeDetection.FallingAndRisingEdge, ISRCallback);

            INTAKE_DHT_SENSOR = (GpioPin)Pi.Gpio[BcmPin.Gpio22];
            //INTAKE_DHT_SENSOR.RegisterInterruptCallback(EdgeDetection.FallingAndRisingEdge, ISRCallback);

            MAIN_DHT_SENSOR = (GpioPin)Pi.Gpio[BcmPin.Gpio16];
            //MAIN_DHT_SENSOR.RegisterInterruptCallback(EdgeDetection.FallingAndRisingEdge, ISRCallback);

            OUTTAKE_DHT_SENSOR = (GpioPin)Pi.Gpio[BcmPin.Gpio25];
            //OUTTAKE_DHT_SENSOR.RegisterInterruptCallback(EdgeDetection.FallingAndRisingEdge, ISRCallback);

            /**Outputs**/
            MAIN_LIGHTING = (GpioPin)Pi.Gpio[BcmPin.Gpio17];
            MAIN_LIGHTING.PinMode = GpioPinDriveMode.Output;

            SIDERIGHT_FAN = (GpioPin)Pi.Gpio[BcmPin.Gpio27];
            SIDERIGHT_FAN.PinMode = GpioPinDriveMode.Output;

            SIDELEFT_FAN = (GpioPin)Pi.Gpio[BcmPin.Gpio23];
            SIDELEFT_FAN.PinMode = GpioPinDriveMode.Output;

        }


        static void AddI2C(string hex) {
            // Register a device on the bus
            var myI2CDevices = Pi.I2C.AddDevice(0x20);

            // Simple Write and Read (there are algo register read and write methods)
            myI2CDevices.Write(0x44);
            var response = myI2CDevices.Read();

            // List registered devices on the I2C Bus
            foreach (var device in Pi.I2C.Devices)
            {
                Console.WriteLine($"Registered I2C Device: {device.DeviceId}");
            }

        }

        // Define the implementation of the delegate;
        static void ISRCallback()
        {
            Console.WriteLine("Pin Activated...");
        }

    }
}
