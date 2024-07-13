using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaySkyTask.Core.Entities;
using PaySkyTask.Core.Result;
using PaySkyTask.Domain.IRepositories;
using PaySkyTask.Infrastructure.BaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasySkyTask.Infrastructure.Repositories;

public class GenericRepository<T> (ApplicationDbContext dbContext , ILogger<GenericRepository<T>> logger) : IGenericRepository<T> where T : BaseEntity
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly DbSet<T> _context = dbContext.Set<T>();
    private readonly ILogger<GenericRepository<T>> _logger = logger;

    public async Task<Result<T>> AddAsync(T entity)
    {
        await _context.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Added new item successfully.");
        return Result<T>.Success(entity);
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        var entity = await _context.FindAsync(id);
        if (entity == null)
        {
            _logger.LogWarning("Item not found: {Id}", id);
            return Result<bool>.NotFound("Item not found");
        }

        _context.Remove(entity);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Deleted item successfully.");
        return Result<bool>.Success(true);
    
}

    public Task<Result<bool>> DeleteAsync(int entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IReadOnlyList<T>>> GetAllAsync()
    {
        var items = await _context.ToListAsync();
        if (items is null)
        {
            _logger.LogWarning("No items found.");
            return Result<IReadOnlyList<T>>.NotFound("No items found.");
        }
        else
        {
            _logger.LogInformation("Retrieved all items successfully.");
          
            return Result<IReadOnlyList<T>>.Success(items);
        }

    }

    public async Task<Result<T>> GetByIdAsync(Guid id)
    {
        var item = await _context.FindAsync(id);
        if (item == null)
        {
            _logger.LogWarning("Item not found: {Id}", id);
            return Result<T>.NotFound("Item not found");
        }

        _logger.LogInformation("Retrieved item by id: {Id}", id);
        return Result<T>.Success(item);
    }

    public async Task<Result<T>> UpdateAsync(T entity)
    {
        _context.Update(entity);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Updated item successfully.");
        return Result<T>.Success(entity);
    }
}