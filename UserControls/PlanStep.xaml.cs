using DatabaseTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WorkoutGenerator.UserControls
{
    /// <summary>
    /// Interaction logic for PlanStep.xaml
    /// </summary>
    public partial class PlanStep : UserControl
    {
        public PlanStep()
        {
            InitializeComponent();
            DataContext = this;
        }

        public Exercise Exercise { get; set; }
        public List<Exercise> PotentialExercises { get; set; }
        public int NumberInList { get; set; }
        public string Reps { get; set; }
        public Guid StepId { get; set; }

        private void RemoveStep(object sender, EventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mainWindow.RemoveStep(sender, e);
        }
    }
}
