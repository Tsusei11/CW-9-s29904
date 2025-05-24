using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs;

public class PrescriptionCreateDto
{
    [Required]
    public DateOnly Date { get; set; }
    [Required]
    public DateOnly DueDate { get; set; }
    [Required]
    public PatientCreateDto Patient { get; set; }
    [Required]
    public int IdDoctor { get; set; }
    [Required]
    public List<MedicamentPrescriptionCreateDto> Medicaments { get; set; }
}

public class MedicamentPrescriptionCreateDto
{
    [Required]
    public int IdMedicament { get; set; }
    public int? Dose { get; set; }
    [Required]
    public string Details { get; set; }
}