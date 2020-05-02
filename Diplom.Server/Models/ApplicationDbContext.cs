﻿using Diplom.Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Diplom.Server.Models
{
    public class ApplicationDbContext : IdentityDbContext<SiteUser, IdentityRole, string>
    {
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<AdditionMenu> AdditionMenus { get; set; }
        

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}