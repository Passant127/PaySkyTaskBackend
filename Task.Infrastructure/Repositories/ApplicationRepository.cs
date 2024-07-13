using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PasySkyTask.Domain.IRepositories;
using PaySkyTask.Domain.Entities;
using PaySkyTask.Domain.Enums;
using PaySkyTask.Infrastructure.BaseContext;

namespace PasySkyTask.Infrastructure.Repositories;

public class ApplicationRepository(ApplicationDbContext context, ILogger<GenericRepository<Applications>> logger , IMemoryCache cache) : GenericRepository<Applications>(context, logger), IApplicationRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<GenericRepository<Applications>> _logger = logger;
    private readonly IMemoryCache _cache = cache;
    public async Task<IReadOnlyList<Applications>> GetApplicationsByVacancyIdAsync(Guid vacancyId)
    {
        string cacheKey = $"ApplicationsByVacancyId_{vacancyId}";

        if (!_cache.TryGetValue(cacheKey, out IReadOnlyList<Applications> applications))
        {
            applications = await _context.Applications
                .Where(a => a.VacancyId == vacancyId)
                .ToListAsync();

            if (applications != null)
            {
                _logger.LogInformation("Applications retrieved successfully for Vacancy ID {VacancyId}. Caching...", vacancyId);

                _cache.Set(cacheKey, applications, TimeSpan.FromMinutes(5));
            }
            else
            {
                _logger.LogError("Failed to retrieve applications for Vacancy ID {VacancyId}.", vacancyId);
            }
        }
        else
        {
            _logger.LogInformation("Applications retrieved from cache for Vacancy ID {VacancyId}.", vacancyId);
        }

        return applications;
    }
    public async Task<IReadOnlyList<Applications>> GetApplicationsByApplicantAsync(Guid applicantId)
    {
        string cacheKey = $"ApplicationsByApplicantId_{applicantId}";

        if (!_cache.TryGetValue(cacheKey, out IReadOnlyList<Applications> applications))
        {
            applications = await _context.Applications
                .Where(a => a.ApplicantId == applicantId)
                .ToListAsync();

            if (applications != null && applications.Count > 0)
            {
                _logger.LogInformation("Applications retrieved successfully for Applicant ID {ApplicantId}. Caching...", applicantId);

                _cache.Set(cacheKey, applications, TimeSpan.FromMinutes(5));
            }
            else
            {
                _logger.LogWarning("No applications found for Applicant ID {ApplicantId}.", applicantId);
            }
        }
        else
        {
            _logger.LogInformation("Applications retrieved from cache for Applicant ID {ApplicantId}.", applicantId);
        }

        return applications;
    }


    public async Task<int> CountApplicationsTodayAsync(Guid applicantId)
    {
        int count = 0;
     

        var today = DateTime.Today;
        count = await _context.Applications
            .CountAsync(a => a.ApplicantId == applicantId && a.ApplicationDate.Date == today);

        if (count == 0)
        {
            _logger.LogWarning("No applications found today for applicant ID {ApplicantId}", applicantId);
          
        }
        else
        {
            _logger.LogInformation("Counted {Count} applications today for applicant ID {ApplicantId}", count, applicantId);
        }

        return count;
    }

    public async Task<bool> CheckEligibilityAsync(Guid vacancyId, Guid applicantId)
    {
        var vacancy = await _context.Vacancies.FindAsync(vacancyId);
       var applications = await _context.Users
                .Where(a => a.Id == applicantId.ToString())
                .FirstOrDefaultAsync();
        if (vacancy == null)
        {
            _logger.LogWarning("Vacancy with ID {VacancyId} not found.", vacancyId);
            return false;
        }

        if (applications == null)
        {
            _logger.LogWarning("Vacancy with ID {VacancyId} not found.", applicantId);
            return false;
        }


        if (vacancy.Status == VacancyStatus.Inactive)
        {
            _logger.LogInformation("Vacancy with ID {VacancyId} is not active.", vacancyId);
            return false;
        }

        return true;
    }

    public async Task<bool> HasAppliedWithin24HoursAsync(Guid applicantId)
    {
        var today = DateTime.Today;
        var yesterday = today.AddDays(-1);

        return await _context.Applications
            .AnyAsync(a => a.ApplicantId == applicantId && a.ApplicationDate >= yesterday);
    }


}


