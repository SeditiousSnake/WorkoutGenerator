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

namespace DatabaseTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ProductContext _context =
            new ProductContext();

        private CollectionViewSource categoryViewSource;
        private ISampleService sampleService;
        private AppSettings settings;

        public MainWindow(ISampleService sampleService,
                          IOptions<AppSettings> settings)
        {
            InitializeComponent();
            this.sampleService = sampleService;
            this.settings = settings.Value;
            categoryViewSource =
                (CollectionViewSource)FindResource(nameof(categoryViewSource));
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

            List<string> targetAreas = _context.Exercises.Select(x => x.BodyPart).Distinct().ToList();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _context.Dispose();
            base.OnClosing(e);
        }
    }
}
