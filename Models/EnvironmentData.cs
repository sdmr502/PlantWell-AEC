namespace PlantWellBgClient.Models
{
    public sealed class EnvironmentData
    {



        public float thresholdTemp { get; set; }
        public float thresholdHum { get; set; }



        public float tempMin { get; set; }
        public float tempMax { get; set; }
        public float humidityMin { get; set; }
        public float humidityMax { get; set; }
        public float targetTemp { get; set; }
        public float targetHum { get; set; }
        public float MainTemp { get; set; }
        public float MainHum { get; set; }
        public float IntakeTemp { get; set; }
        public float IntakeHum { get; set; }
        public float OuttakeTemp { get; set; }
        public float OuttakeHum { get; set; }

        /**
         * 
         */
        public float Lighting { get; set; }
        public float IntakeEntFan { get; set; }
        public float IntakeExitTopFan { get; set; }
        public float IntakeExitBottomFan { get; set; }
        public float OuttakeRecircFan { get; set; }
        public float OuttakeExitFan { get; set; }
        public float OuttakeEntFan { get; set; }
        public float LightCoolingFan { get; set; }
        public float Dehumidifier { get; set; }
        public float Humidifier { get; set; }
        public float Option { get; set; }
        public string Version { get; set; }
    }

    public sealed class DHTData
    {
 
        public double Temp { get; set; }
        public double Hum { get; set; }

    }
}
