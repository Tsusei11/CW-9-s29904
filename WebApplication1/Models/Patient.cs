using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Patient
{
    [Key]
    public int IdPatient { get; set; }
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;
    [MaxLength(100)]
    public string LastName { get; set; } = null!;
    [Required]
    public DateOnly BirthDate { get; set; }
    
    public virtual ICollection<Prescription> Prescriptions { get; set; } = null!;
}