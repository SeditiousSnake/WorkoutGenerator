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
        private ISampleService sampleService;
        private AppSettings settings;
        private Random rand;

        public MainWindow(ISampleService sampleService,
                          IOptions<AppSettings> settings)
        {
            InitializeComponent();
            this.sampleService = sampleService;
            this.settings = settings.Value;
            categoryViewSource =
                (CollectionViewSource)FindResource(nameof(categoryViewSource));
            rand = new Random();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Exercises.RemoveRange(_context.Exercises);
            _context.SaveChanges();
            _context.Exercises.AddRange(sampleService.GetExercisesFromExcel());
            _context.SaveChanges();

            categoryViewSource.Source =
                _context.Exercises.Local.ToObservableCollection();

            TargetAreaDropdown.ItemsSource = _context.Exercises.Select(x => x.TargetArea).Distinct().ToList();
            BodyPartDropdown.ItemsSource = _context.Exercises.Select(x => x.BodyPart).Distinct().ToList();
            TypeDropdown.ItemsSource = _context.Exercises.Select(x => x.Type).Distinct().ToList();
            SetsDropdown.ItemsSource = _context.Exercises.Select(x => x.Sets).Distinct().ToList();
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

        private void Type_SelectionChanged(object sender, EventArgs e)
        {
            SetsDropdown.ItemsSource = _context.Exercises
                .Where(x => x.BodyPart == (string)BodyPartDropdown.SelectedItem
                && x.TargetArea == (string)TargetAreaDropdown.SelectedItem
                && x.Type == (string) TypeDropdown.SelectedItem)
                .Select(x => x.Sets)
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
            ResultsPanel.Children.Add(new PlanStep
            {
                Exercise = randomExercise,
                NumberInList = ResultsPanel.Children.Count + 1
            });
        }
    }
}
