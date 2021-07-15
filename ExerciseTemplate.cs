using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutGenerator
{
    public class ExerciseTemplate
    {
        public int ExerciseTemplateId { get; set; }
        public string Name { get; set; }
        public byte[] File { get; set; }
    }
}
