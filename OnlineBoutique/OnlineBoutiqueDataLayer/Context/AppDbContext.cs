using Microsoft.EntityFrameworkCore;
using OnlineBoutiqueDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBoutiqueDataLayer.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure many-to-many without join entity
            modelBuilder.Entity<Item>()
                .HasMany(i => i.Categories)
                .WithMany(c => c.Items);
        }
    }
}
