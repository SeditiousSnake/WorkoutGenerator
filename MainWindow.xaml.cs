using MahApps.Metro.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorkoutGenerator.UserControls;

namespace DatabaseTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly ProductContext _context =
            new ProductContext();

        private CollectionViewSource categoryViewSource;
        private IExcelService excelService;
        private AppSettings settings;
        private Random rand;
        private List<PlanStep> planSteps;

        public MainWindow(IExcelService excelService,
                          IOptions<AppSettings> settings)
        {
            InitializeComponent();
            this.excelService = excelService;
            this.settings = settings.Value;
            categoryViewSource =
                (CollectionViewSource)FindResource(nameof(categoryViewSource));
            rand = new Random();
            planSteps = new List<PlanStep>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Exercises.RemoveRange(_context.Exercises);
            _context.SaveChanges();
            _context.Exercises.AddRange(excelService.GetExercisesFromExcel());
            _context.SaveChanges();

            categoryViewSource.Source =
                _context.Exercises.Local.ToObservableCollection();

            TargetAreaDropdown.ItemsSource = _context.Exercises.Select(x => x.TargetArea).Distinct().ToList();
            BodyPartDropdown.ItemsSource = _context.Exercises.Select(x => x.BodyPart).Distinct().ToList();
            TypeDropdown.ItemsSource = _context.Exercises.Select(x => x.Type).Distinct().ToList();
            IntensityDropdown.ItemsSource = new List<string>() { "Beginner", "Normal", "Don", "Don High", "Power"};
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _context.Dispose();
            base.OnClosing(e);
        }

        private void BodyPart_SelectionChanged(object sender, EventArgs e)
        {
            TargetAreaDropdown.ItemsSource = _context.Exercises.Where(x => x.BodyPart == (string) BodyPartDropdown.SelectedItem).Select(x => x.TargetArea).Distinct().ToList();
        }

        private void TargetArea_SelectionChanged(object sender, EventArgs e)
        {
            TypeDropdown.ItemsSource = _context.Exercises
                .Where(x => x.BodyPart == (string)BodyPartDropdown.SelectedItem
                && x.TargetArea == (string)TargetAreaDropdown.SelectedItem)
                .Select(x => x.Type)
                .Distinct()
                .ToList();
        }

        private void AddResult(object sender, EventArgs e)
        {
            var possibleExercises = _context.Exercises
                .Where(x => x.BodyPart == (string)BodyPartDropdown.SelectedItem
                && x.TargetArea == (string)TargetAreaDropdown.SelectedItem
                && x.Type == (string)TypeDropdown.SelectedItem)
                .ToList();
            Exercise randomExercise = possibleExercises[rand.Next(possibleExercises.Count)];
            string repsString = GetRepsString(randomExercise);
            var newStep = new PlanStep
            {
                Exercise = randomExercise,
                PotentialExercises = possibleExercises,
                NumberInList = ResultsPanel.Children.Count + 1,
                Reps = repsString,
                StepId = Guid.NewGuid()
            };

            planSteps.Add(newStep);
            ResultsPanel.Children.Add(newStep);
        }

        private void ExportPlan(object sender, EventArgs e)
        {
            excelService.ExportToExcel(planSteps);
        }

        private string GetRepsString(Exercise exercise)
        {
            var repsString = "";
            //TODO: Need to stop basing this off current dropdown selection, instead use what was assigned to the exercise step
            switch (IntensityDropdown.SelectedItem)
            {
                case ("Beginner"):
                    repsString = exercise.Beginner;
                    break;
                case ("Normal"):
                    repsString = exercise.Normal;
                    break;
                case ("Don"):
                    repsString = exercise.Don;
                    break;
                case ("Don High"):
                    repsString = exercise.DonHigh;
                    break;
                case ("Power"):
                    repsString = exercise.Power;
                    break;
            }
            return repsString;
        }

        public void RemoveStep(object sender, EventArgs e)
        {
            Guid stepId = ((Guid)((Button)sender).Tag);
            var stepToRemove = planSteps.Single(x => x.StepId == stepId);
            planSteps.Remove(stepToRemove);
            ResultsPanel.Children.Remove(stepToRemove);
        }

        public void Exercise_SelectionChanged(object sender, EventArgs e)
        {
            Guid stepId = ((Guid)((ComboBox)sender).Tag);
            var stepToUpdate = planSteps.Single(x => x.StepId == stepId);
            stepToUpdate.Reps = GetRepsString(stepToUpdate.Exercise);
        }
    }
}
