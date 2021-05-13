using PlantWellBgClient.Models;
using Sensors.Dht;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using System.Runtime.Caching;
using System.Diagnostics;
using Windows.Foundation;

namespace PlantWellBgClient.Services
{
    public sealed class PiDriver
    {

        /**
         * Define inputs 
         */
        private GpioPin MOISTURE_SENSOR_1;

        private GpioPin INTAKE_DHT_SENSOR;
        private DHTData intakeDHTData;

        private GpioPin OUTTAKE_DHT_SENSOR;
        private DHTData outtakeDHTData;

        private GpioPin MAIN_DHT_SENSOR;
        private DHTData mainDHTData;

        /**
         * Define Outputs 
         */
        private GpioPin MAIN_LIGHTING;
        private GpioPin SIDERIGHT_FAN;
        private GpioPin SIDELEFT_FAN;

        public PiDriver()
        {



            MOISTURE_SENSOR_1 = GpioController.GetDefault().OpenPin(21, GpioSharingMode.Exclusive);

            INTAKE_DHT_SENSOR = GpioController.GetDefault().OpenPin(22, GpioSharingMode.Exclusive);

            OUTTAKE_DHT_SENSOR = GpioController.GetDefault().OpenPin(25, GpioSharingMode.Exclusive);

            MAIN_DHT_SENSOR = GpioController.GetDefault().OpenPin(16, GpioSharingMode.Exclusive);


            /**
             * Define Outputs 
             */
            MAIN_LIGHTING = GpioController.GetDefault().OpenPin(17);
            SIDERIGHT_FAN = GpioController.GetDefault().OpenPin(27);
            SIDELEFT_FAN = GpioController.GetDefault().OpenPin(23);



        }





    }
}
