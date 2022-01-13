using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarsRover.Models
{
    public class RoverViewModel
    {
        public int Index { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }
        public string Orientation { get; set; }
        public string Move { get; set; }
    }

    public class RoverPositionModel
    {
        public int? X { get; set; }
        public int? Y { get; set; }
        public string Orientation { get; set; }
    }
}