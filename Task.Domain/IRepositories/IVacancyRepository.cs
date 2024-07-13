using Microsoft.EntityFrameworkCore;
using PaySkyTask.Core.Result;
using PaySkyTask.Domain.Entities;
using PaySkyTask.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasySkyTask.Domain.IRepositories;

public interface IVacancyRepository :IGenericRepository<Vacancy>
{
    Task<Result<IReadOnlyList<Vacancy>>> SearchVacanciesAsync(string searchTerm);
    public IEnumerable<Vacancy> GetExpiredVacancies();

    Task<Result<bool>> ArchiveExpiredVacanciesAsync();
}
