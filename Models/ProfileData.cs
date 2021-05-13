using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantWellBgClient.Models
{
    class ProfileData
    {
        public int id { get; set; }
        public float tempMin { get; set; }
        public float tempMax { get; set; }

        public float humMin { get; set; }

        public float humMax { get; set; }

        public float targetTemp { get; set; }

        public float targetHum { get; set; }

        public float tempThreshold { get; set; }

        public float humThreshold { get; set; }

        public bool active { get; set; }

        public DateTime daylightFrom { get; set; }

        public DateTime daylightTo { get; set; }

        public float cutoffTempOveride { get; set; }


    }
}
