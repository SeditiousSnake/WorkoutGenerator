using DatabaseTest;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutGenerator
{
    public class SelectedExerciseModel
    {
        public Exercise Exercise { get; set; }
        public List<Exercise> PotentialExercises { get; set; }
        public int NumberInList { get; set; }
    }
}
