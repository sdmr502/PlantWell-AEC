using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantWellBgClient.Models
{
    public sealed class ErrorData
    {
        public float Id { get; set; }
        public int Level { get; set; }
        public string Module { get; set; }
        public string Exception { get; set; }
    }
}
