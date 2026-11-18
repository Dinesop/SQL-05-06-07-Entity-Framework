using Microsoft.EntityFrameworkCore;
using FailSafeRentals.Core.Models;

namespace FailSafeRentals.Data;

public class FailSafeRentalsDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Vehicle> Vehicles { get; set; } = null!;
    public DbSet<Rental> Rentals { get; set; } = null!;
    public DbSet<ExtraFeature> ExtraFeatures { get; set; } = null!;
    public DbSet<RentalStation> RentalStations { get; set; }
    public DbSet<FleetVehicle> FleetVehicles { get; set; }
    public DbSet<DamageReport> DamageReports { get; set; }
    public DbSet<MaintenanceTag> MaintenanceTags { get; set; }

    public FailSafeRentalsDbContext()
    {
        
    }
    public FailSafeRentalsDbContext(DbContextOptions<FailSafeRentalsDbContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer
            (
            @"Server=(localdb)\MSSQLLocalDB;Database=FailSafeRentals;Trusted_Connection=True;TrustServerCertificate=True;")
            .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, Microsoft.Extensions.Logging.LogLevel.Information)
            .EnableSensitiveDataLogging(); // OBS mycket dåligt att ha med, visar parametervärden
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

        // Uppgift 9: Kaskad delete
        modelBuilder.Entity<FleetVehicle>()
            .HasOne(v => v.RentalStation) // En fleet vehicle pekar på en rental station
            .WithMany(s => s.Vehicles) // En station pekar på flera Vehicles
            .HasForeignKey(v => v.RentalStationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DamageReport>()
            .HasOne(d => d.FleetVehicle) // En fleet vehicle pekar på en rental station
            .WithMany(v => v.DamageReports) // En station pekar på flera Vehicles
            .HasForeignKey(d => d.FleetVehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Uppgift 11
        modelBuilder.Entity<RentalStation>()
            .HasData(
                new RentalStation { Id = 1, StationCode = "GOT-01", City = "Göteborg", StreetAddress = "Landvetter Flygplats" },
                new RentalStation { Id = 2, StationCode = "STO-01", City = "Stockholm", StreetAddress = "Arlanda Flygplats" },
                new RentalStation { Id = 3, StationCode = "MMX-01", City = "Malmö", StreetAddress = "Sturups Flygplats" }

            );

        // Uppgift 12
            //modelBuilder.Entity<RentalStation>()
            //    .HasData(
            //        new RentalStation { Id = 4, StationCode = "OSL-01", City = "Oslo", StreetAddress = "Gardermoen 1" }
            //    );
    }
}
