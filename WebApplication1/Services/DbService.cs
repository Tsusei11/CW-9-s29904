using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Models;

namespace WebApplication1.Services;

public interface IDbService
{
    public Task<PrescriptionDetailsDto> CreatePrescription(PrescriptionCreateDto prescriptionData);
    public Task<PatientDetailsDto> GetPatient(int IdPatient);
}

public class DbService(AppDbContext data) : IDbService
{
    public async Task<PrescriptionDetailsDto> CreatePrescription(PrescriptionCreateDto prescriptionData)
    {
        //PATIENT
        var patient = data.Patients.FirstOrDefault(p => p.IdPatient == prescriptionData.Patient.IdPatient);

        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = prescriptionData.Patient.FirstName,
                LastName = prescriptionData.Patient.LastName,
                BirthDate = prescriptionData.Patient.BirthDate,
            };
            
            data.Patients.Add(patient);
            await data.SaveChangesAsync();
        }
        
        //MEDICAMENTS

        if (prescriptionData.Medicaments.Count == 0)
        {
            throw new NoMedicamentsProvidedException("No medicaments provided");
        }

        foreach (var medicamentPrescription in prescriptionData.Medicaments)
        {
            var medicament = data.Medicaments.FirstOrDefault(m => m.IdMedicament == medicamentPrescription.IdMedicament);
            
            if (medicament == null)
                throw new NotFoundException($"Medicament {medicamentPrescription.IdMedicament} not found");
        }
        
        //DOCTOR
        var doctor = data.Doctors.FirstOrDefault(d => d.IdDoctor == prescriptionData.IdDoctor);
        
        if (doctor == null)
            throw new NotFoundException($"Doctor {prescriptionData.IdDoctor} not found");
        
        //DATES
        if (prescriptionData.Date > prescriptionData.DueDate)
        {
            throw new IncorrectDateException("Due date must be after prescription date");
        }
        
        //ADD DATA
        var prescription = new Prescription
        {
            Date = prescriptionData.Date,
            DueDate = prescriptionData.DueDate,
            IdPatient = patient.IdPatient,
            IdDoctor = doctor.IdDoctor
        };
        
        await data.AddAsync(prescription);
        await data.SaveChangesAsync();

        List<PrescriptedMedicamentGetDto> medicaments = [];
        
        foreach (var medicamentPrescriptionData in prescriptionData.Medicaments)
        {
            var medicamentPrescription = new PrescriptionMedicament()
            {
                IdPrescription = prescription.IdPrescription,
                IdMedicament = medicamentPrescriptionData.IdMedicament,
                Details = medicamentPrescriptionData.Details,
                Dose = medicamentPrescriptionData.Dose,
            };
            
            var medicament = data.Medicaments.First(m => m.IdMedicament == medicamentPrescription.IdMedicament);
            medicaments.Add(new PrescriptedMedicamentGetDto
            {
                IdMedicament = medicamentPrescription.IdMedicament,
                Name = medicament.Name,
                Description = medicament.Description,
                Details = medicamentPrescription.Details,
                Dose = medicamentPrescription.Dose,
            });
            
            await data.PrescriptionMedicaments.AddAsync(medicamentPrescription);
            await data.SaveChangesAsync();
        }

        return new PrescriptionDetailsDto()
        {
            IdPrescription = prescription.IdPrescription,
            Date = prescription.Date,
            DueDate = prescription.DueDate,
            Doctor = new DoctorDetailsDto
            {
                IdDoctor = doctor.IdDoctor,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Email = doctor.Email
            },
            Medicaments = medicaments
        };
    }

    public async Task<PatientDetailsDto> GetPatient(int IdPatient)
    {
        var result = await data.Patients.Select(p => new PatientDetailsDto
        {
            IdPatient = p.IdPatient,
            FirstName = p.FirstName,
            LastName = p.LastName,
            BirthDate = p.BirthDate,
            Prescriptions = data.Prescriptions.Where(pr => pr.IdPatient == p.IdPatient)
                .Select(pr => new PrescriptionDetailsDto()
                {
                    IdPrescription = pr.IdPrescription,
                    Date = pr.Date,
                    DueDate = pr.DueDate,
                    Doctor = data.Doctors.Where(d => d.IdDoctor == pr.IdDoctor)
                        .Select(d => new DoctorDetailsDto
                        {
                            IdDoctor = d.IdDoctor,
                            FirstName = d.FirstName,
                            LastName = d.LastName,
                            Email = d.Email
                        }).Single(),
                    Medicaments = data.PrescriptionMedicaments.Where(pm => pm.IdPrescription == pr.IdPrescription)
                        .Select(pm => new PrescriptedMedicamentGetDto
                        {
                            IdMedicament = pm.IdMedicament,
                            Name = data.Medicaments.First(m => m.IdMedicament == pm.IdMedicament).Name,
                            Description = data.Medicaments.First(m => m.IdMedicament == pm.IdMedicament).Description,
                            Details = pm.Details,
                            Dose = pm.Dose,
                        }).ToList(),
                }).ToList(),
        }).FirstOrDefaultAsync(p => p.IdPatient == IdPatient);
        
        return result ?? throw new NotFoundException($"Patient {IdPatient} not found");
    }
}