using Diplom.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace Diplom.Mobile
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<Product> Menu { get; set; }

        public DbSet<Review> Review { get; set; }

        //Через конструктор объект этого класса получает в переменную _databasePath путь к базе данных
        private readonly string _databasePath;

        public ApplicationContext()
        {
            const string dbFileName = "clientapp.db";
            _databasePath = DependencyService.Get<IPath>().GetDatabasePath(dbFileName);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databasePath}");
        }
    }
}