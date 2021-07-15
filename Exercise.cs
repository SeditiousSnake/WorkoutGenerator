using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DatabaseTest
{
    public class Exercise
    {
        public int ExerciseId { get; set; }
        public string Name { get; set; }
        public string BodyPart { get; set; }
        public string TargetArea { get; set; }
        public string Type { get; set; }
        public string Sets { get; set; }
        public string Beginner { get; set; }
        public string Normal { get; set; }
        public string Don { get; set; }
        public string DonHigh { get; set; }
        public string Power { get; set; }
    }
}
