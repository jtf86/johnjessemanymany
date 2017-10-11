using Microsoft.EntityFrameworkCore;
using TravelBlog.Models;

namespace TravelBlog.Models
{
    public class TravelBlogContext : DbContext
    {
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Experience> Experiences { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<LocationPerson> LocationPerson { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Location to Person Many-to-Many:
            modelBuilder.Entity<LocationPerson>()
                        .HasKey(x => new { x.LocationId, x.PersonId });
            modelBuilder.Entity<LocationPerson>()
                        .HasOne(x => x.Location)
                        .WithMany(x => x.LocationPerson)
                        .HasForeignKey(x => x.LocationId);
            modelBuilder.Entity<LocationPerson>()
                        .HasOne(x => x.Person)
                        .WithMany(x => x.LocationPerson)
                        .HasForeignKey(x => x.PersonId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseMySql(@"Server=localhost;Port=8889;database=travelblog;uid=root;pwd=root;");
    }
}