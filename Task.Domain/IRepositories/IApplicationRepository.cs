using PaySkyTask.Core.Result;
using PaySkyTask.Domain.Entities;
using PaySkyTask.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasySkyTask.Domain.IRepositories;

public interface IApplicationRepository: IGenericRepository<Applications> 
{
    Task<IReadOnlyList<Applications>> GetApplicationsByVacancyIdAsync(Guid vacancyId);
    Task<IReadOnlyList<Applications>> GetApplicationsByApplicantAsync(Guid applicantId);
    Task<int> CountApplicationsTodayAsync(Guid applicantId);
    Task<bool> CheckEligibilityAsync(Guid vacancyId, Guid applicantId);
    Task<bool> HasAppliedWithin24HoursAsync(Guid applicantId);

}
