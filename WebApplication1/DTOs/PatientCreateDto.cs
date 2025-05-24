using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs;

public class PatientCreateDto
{
    [Required]
    public int IdPatient { get; set; }
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }
    [Required]
    public DateOnly BirthDate { get; set; }
}