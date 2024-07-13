using PaySkyTask.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasySkyTask.Application.DTOS;

public class VacancyRequestDto
{

    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }


    [Required]
    public DateTime ExpiryDate { get; set; }
    [Required]
    public int MaxApplications { get; set; }

    [Required]
    public int CurrentApplications { get; set; }

    [Required]
    public VacancyStatus Status { get; set; }

    [Required]
    public Guid EmployerId { get; set; }
}
