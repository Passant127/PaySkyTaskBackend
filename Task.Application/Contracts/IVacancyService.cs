using Clinic.Application.Contracts;
using PasySkyTask.Application.DTOS;
using PaySkyTask.Core.Entities;
using PaySkyTask.Core.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasySkyTask.Application.Contracts;

public interface IVacancyService : ILifeTime, IScopedService
{
     Task<Result<VacancyResponseDto>> GetVacancyByIdAsync(Guid vacancyId);
    Task<Result<IReadOnlyList<VacancyResponseDto>>> GetAllVacanciesAsync();
    Task<Result<VacancyResponseDto>> AddVacancyAsync(VacancyRequestDto vacancy);
    Task<Result<VacancyResponseDto>> UpdateVacancyAsync(VacancyRequestDto vacancy);
    Task<Result<bool>> DeleteVacancyAsync(Guid vacancyId);
    Result<bool> ArchiveExpiredVacanciesAsync();
    Task<Result<IReadOnlyList<VacancyResponseDto>>> SearchVacanciesAsync(string searchTerm);
    Task<Result<bool>> PostVacancyAsync(Guid vacancyId);
    Task<Result<bool>> DeactivateVacancyAsync(Guid vacancyId);

}
