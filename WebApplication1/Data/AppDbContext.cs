using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class AppDbContext : DbContext
{
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
	    
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var doctors = new List<Doctor>
        {
            new()
            {
                IdDoctor = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com",
            },
            new()
            {
                IdDoctor = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "janedoe@gmail.com",
            }
        };

        var patients = new List<Patient>
        {
            new()
            {
                IdPatient = 1,
                FirstName = "Jim",
                LastName = "Hopper",
                BirthDate = DateOnly.FromDateTime(DateTime.Parse("20-05-1949"))
            },
            new()
            {
                IdPatient = 2,
                FirstName = "Joyce",
                LastName = "Bayers",
                BirthDate = DateOnly.FromDateTime(DateTime.Parse("13-12-1949"))
            }
        };

        var medicaments = new List<Medicament>
        {
            new()
            {
                IdMedicament = 1,
                Name = "Some cool drug",
                Description = "Some description",
                Type = "Some type"
            },
            new()
            {
                IdMedicament = 2,
                Name = "Some very cool drug",
                Description = "Some description",
                Type = "Some type"
            }
        };

        var prescriptions = new List<Prescription>
        {
            new()
            {
                IdPrescription = 1,
                IdDoctor = 1,
                IdPatient = 2,
                Date = DateOnly.FromDateTime(DateTime.Parse("14-05-1987")),
                DueDate = DateOnly.FromDateTime(DateTime.Parse("12-12-1987")),
            },
            new()
            {
                IdPrescription = 2,
                IdDoctor = 2,
                IdPatient = 1,
                Date = DateOnly.FromDateTime(DateTime.Parse("27-06-1987")),
                DueDate = DateOnly.FromDateTime(DateTime.Parse("27-09-1987")),
            }
        };

        var prescriptionMedicament = new List<PrescriptionMedicament>
        {
            new()
            {
                IdMedicament = 1,
                IdPrescription = 1,
                Details = "Some details",
                Dose = 10
            },
            new()
            {
                IdMedicament = 2,
                IdPrescription = 1,
                Details = "Some details"
            },
            new()
            {
                IdMedicament = 2,
                IdPrescription = 2,
                Details = "Some details",
                Dose = 2
            }
        };
        
        modelBuilder.Entity<Doctor>().HasData(doctors);
        modelBuilder.Entity<Patient>().HasData(patients);
        modelBuilder.Entity<Medicament>().HasData(medicaments);
        modelBuilder.Entity<Prescription>().HasData(prescriptions);
        modelBuilder.Entity<PrescriptionMedicament>().HasData(prescriptionMedicament);
    }
}