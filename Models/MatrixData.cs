using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantWellBgClient.Models
{
    public sealed class MatrixData
    {
        public int Id { get; set; }
        public bool Lighting { get; set; }
        public bool IntakeEntFan { get; set; }
        public bool IntakeExitTopFan { get; set; }
        public bool IntakeExitBottomFan { get; set; }
        public bool OuttakeRecircFan { get; set; }
        public bool OuttakeExitFan { get; set; }
        public bool OuttakeEntFan { get; set; }
        public bool LightCoolingFan { get; set; }
        public bool Dehumidifier { get; set; }
        public bool Humidifier { get; set; }
    }

    public sealed class  MatrixPrediction
    {
        public int Id { get; set; }
        public double Prediction { get; set; }
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
    }
}
