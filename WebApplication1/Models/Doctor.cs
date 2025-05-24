using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Doctor
{
    [Key]
    public int IdDoctor { get; set; }
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;
    [MaxLength(100)]
    public string LastName { get; set; } = null!;
    [RegularExpression(@"^[a-zA-Z0-9_\-\.]+@[a-z]+\.[a-z]+$")]
    public string Email { get; set; } = null!;
    
    public virtual ICollection<Prescription> Prescriptions { get; set; } = null!;
}