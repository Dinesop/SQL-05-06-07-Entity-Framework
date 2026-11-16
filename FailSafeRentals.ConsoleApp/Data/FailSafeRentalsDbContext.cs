using Microsoft.EntityFrameworkCore;

using FailSafeRentals.ConsoleApp.Models;

namespace FailSafeRentals.ConsoleApp.Data;

public class FailSafeRentalsDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Vehicle> Vehicles { get; set; } = null!;
    public DbSet<Rental> Rentals { get; set; } = null!;
    public DbSet<ExtraFeature> ExtraFeatures { get; set; } = null!;
    public DbSet<RentalStation> RentalStations { get; set; }
    public DbSet<FleetVehicle> FleetVehicles { get; set; }
    public DbSet<DamageReport> DamageReports { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=(localdb)\MSSQLLocalDB;Database=FailSafeRentals;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. Unikt index på registreringsnummer
        modelBuilder.Entity<Vehicle>()
            .HasIndex(v => v.RegistrationNumber)
            .IsUnique();

        // 2. Explicit precision för pengar i LocalDB
        modelBuilder.Entity<Vehicle>()
            .Property(v => v.DailyRate)
            .HasPrecision(10, 2);

        // 3. Shadow Property för SecOps-spårbarhet
        modelBuilder.Entity<Rental>()
            .Property<DateTime>("CreatedAt")
            .HasDefaultValueSql("GETUTCDATE()");

        // 4. Skydda uthyrningshistorik mot oavsiktlig radering 
        modelBuilder.Entity<Rental>()
            .HasOne(r => r.Customer)
            .WithMany(c => c.Rentals)
            .OnDelete(DeleteBehavior.Restrict);

        // Uppgift 7: Unika index & decimalprecision (10,2)
        modelBuilder.Entity<RentalStation>()
            .HasIndex(s => s.StationCode)
            .IsUnique();

        modelBuilder.Entity<FleetVehicle>()
            .HasIndex(v => v.RegistrationNumber)
            .IsUnique();

        modelBuilder.Entity<DamageReport>()
            .Property(d => d.EstimatedCost)
            .HasPrecision(10, 2);

        // Uppgift 8: Shadow Properties
        modelBuilder.Entity<DamageReport>()
            .Property<DateTime>("AuditLoggedAt")
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
