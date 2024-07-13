using Clinic.Application.Contracts;
using PasySkyTask.Application.DTOS;
using PaySkyTask.Core.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasySkyTask.Application.Contracts;

public interface IApplicationService : ILifeTime, IScopedService
{
    Task<Result<IReadOnlyList<ApplicationResponseDto>>> GetApplicationsByApplicantAsync(Guid applicantId);
    Task<Result<IReadOnlyList<ApplicationResponseDto>>> GetApplicantsForVacancyAsync(Guid vacancyId);
    Task<Result<ApplicationResponseDto>> GetApplicationByIdAsync(Guid applicationId);
    Task<Result<ApplicationResponseDto>> ApplyToVacancyAsync(ApplicationRequestDto request);
    Task<Result<bool>> CanApplyToVacancyAsync(Guid vacancyId, Guid applicantId);
  
}