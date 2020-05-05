using Diplom.Common.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Diplom.Mobile
{
    class ApplicationContext : DbContext
    {
        //Через конструктор объект этого класса получает в переменную _databasePath путь к базе данных
        private string _databasePath;

        public DbSet<Product> Menu { get; set; }
        public DbSet<Review> Review { get; set; }

        public ApplicationContext(string databasePath)
        {
            _databasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databasePath}");
        }
    }
}
