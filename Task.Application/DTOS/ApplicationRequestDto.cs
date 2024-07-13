using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasySkyTask.Application.DTOS;

public class ApplicationRequestDto
{
    [Required]
    public Guid ApplicantId { get; set; }

    [Required]
    public Guid VacancyId { get; set; }

    [Required]
    public DateTime ApplicationDate{ get; set;}
}
