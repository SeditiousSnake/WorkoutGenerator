using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutGenerator
{
    public class WorkoutStep
    {
        public string BodyPart { get; set; }
        public string TargetArea { get; set; }
        public string Type { get; set; }
        public string Sets { get; set; }
        public string Reps { get; set; }
    }
}
