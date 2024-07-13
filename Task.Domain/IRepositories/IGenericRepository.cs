using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaySkyTask.Core.Entities;
using PaySkyTask.Core.Result;

namespace PaySkyTask.Domain.IRepositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<Result<T>> GetByIdAsync(Guid id);
    Task<Result<IReadOnlyList<T>>> GetAllAsync();
    Task<Result<T>> AddAsync(T entity);
    Task<Result<T>> UpdateAsync(T entity);
    Task<Result<bool>> DeleteAsync(Guid id);
}
