using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOS;
public class RegisterRequestDto
{
    [Required]
    public string FirstName { get; set; } = "";
    [Required]
    public string LastName { get; set; } = "";
    [Required]
    public string UserName { get; set; } = "";
    [Required]
    public string Email { get; set; } = "";
    [Required]
    public string Password { get; set; } = "";
    [Required]
    public string Gender { get; set; } = "";
    [Required]    
    public string PhoneNumber { get; set; } = "";
}
