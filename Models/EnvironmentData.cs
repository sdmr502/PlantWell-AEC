namespace PlantWell.Models
{
    public class EnvironmentData
    {
        public float MainTemp { get; set; }
        public float MainHum { get; set; }
        public float IntakeTemp { get; set; }
        public float IntakeHum { get; set; }
        public float OuttakeTemp { get; set; }
        public float OuttakeHum { get; set; }
        public float MainLight { get; set; }
        public float IntakeFan { get; set; }
        public float OuttakeFan { get; set; }
        public float Option { get; set; }
        public string Version { get; set; }
    }
}
