using DatabaseTest;
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
        public int NumberInList { get; set; }
    }
}
