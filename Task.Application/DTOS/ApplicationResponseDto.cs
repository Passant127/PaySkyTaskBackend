using PaySkyTask.Domain.Entities;
using PaySkyTask.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasySkyTask.Application.DTOS;

public class ApplicationResponseDto : ApplicationRequestDto
{
    public Guid Id { get; set; }
    public ApplicationStatus Status { get; set; }

}
