using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PasySkyTask.Domain.IRepositories;
using PaySkyTask.Core.Result;
using PaySkyTask.Domain.Entities;
using PaySkyTask.Domain.Enums;
using PaySkyTask.Infrastructure.BaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasySkyTask.Infrastructure.Repositories;

public class VacancyRepository(ApplicationDbContext context, ILogger<GenericRepository<Vacancy>> logger) : GenericRepository<Vacancy>(context, logger), IVacancyRepository
{

    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<GenericRepository<Vacancy>> _logger = logger;


    public async Task<Result<bool>> ArchiveExpiredVacanciesAsync()
    {
        _logger.LogInformation("Archiving expired vacancies");

        var expiredVacancies = await _context.Vacancies.Where(v => v.ExpiryDate < DateTime.UtcNow)
                                                       .ToListAsync();

        _context.Vacancies.RemoveRange(expiredVacancies);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Archived {Count} expired vacancies", expiredVacancies.Count);
        return Result<bool>.Success(true);
    }

    public IEnumerable<Vacancy> GetExpiredVacancies()
    {
     
            var today = DateTime.UtcNow.Date;
            return _context.Vacancies.Where(v => v.ExpiryDate < today && !v.IsArchived).ToList();
        

    }

    public async Task<Result<IReadOnlyList<Vacancy>>> SearchVacanciesAsync(string searchTerm)
    {
        _logger.LogInformation("Searching vacancies with term: {SearchTerm}", searchTerm);

        var vacancies = await _context.Vacancies.Where(v => v.Title.Contains(searchTerm) || v.Description.Contains(searchTerm))
                                                 .ToListAsync();

        _logger.LogInformation("Found {Count} vacancies matching search term: {SearchTerm}", vacancies.Count, searchTerm);
        return Result<IReadOnlyList<Vacancy>>.Success(vacancies);
    }

 

 
}


