using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using WebApplication2.Domain;
using WebApplication2.Services;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> User { get; set; }
    public DbSet<Request> Request { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasOne(a => a.Manager).WithMany(a => a.ManagedUsers).IsRequired(false);
        modelBuilder.Entity<User>().HasOne(a => a.Request).WithOne(a => a.Applicant).IsRequired(false);
    }
}