using Microsoft.EntityFrameworkCore;
using WorkoutGenerator;

namespace DatabaseTest
{
    class ProductContext : DbContext
    {
        public DbSet<ExerciseTemplate> ExerciseTemplates { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "Data Source=exercises.db");
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
