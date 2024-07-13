using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasySkyTask.Application.DTOS;

public class VacancyResponseDto:VacancyRequestDto
{
    public Guid Id { get; set; }
}
