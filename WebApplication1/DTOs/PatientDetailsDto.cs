namespace WebApplication1.DTOs;

public class PatientDetailsDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly BirthDate { get; set; }
    public List<PrescriptionDetailsDto> Prescriptions { get; set; }
}