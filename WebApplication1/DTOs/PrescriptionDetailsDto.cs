namespace WebApplication1.DTOs;

public class PrescriptionDetailsDto
{
    public int IdPrescription { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }
    public DoctorDetailsDto Doctor { get; set; }
    public List<PrescriptedMedicamentGetDto> Medicaments { get; set; }
}