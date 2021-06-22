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
        public string Reps {
            get { return (string)GetValue(RepsProperty); }
            set { SetValue(RepsProperty, value); }
        }
        public static readonly DependencyProperty RepsProperty =
            DependencyProperty.Register("Reps", typeof(string), typeof(PlanStep), new UIPropertyMetadata(""));
        public Guid StepId { get; set; }

        //TODO: Can we remove this with two-way binding?
        private void RemoveStep(object sender, EventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mainWindow.RemoveStep(sender, e);
        }

        //TODO: See if better Two-Way binding can eliminate the need for this.
        private void Exercise_SelectionChanged(object sender, EventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mainWindow.Exercise_SelectionChanged(sender, e);
        }
    }
}
