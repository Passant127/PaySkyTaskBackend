using AutoMapper;
using AutoMapper.QueryableExtensions;
using PaySkyTask.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySkyTask.Application.ExtensionForServices;

public static class IQueryableExtensions
{
    public static async Task<PaginationResult<T>> GetAllWithPagination<T>(this IQueryable<T> query, int itemCount, int index)
    {
        var totalCount = await query.CountAsync();
        var endIndex = index + itemCount;
        if (itemCount > totalCount)
        {
            itemCount = 0;
        }
        if (index > totalCount)
        {
            index = 0;
        }
        if (itemCount <= 0)
        {
            itemCount = totalCount;
            endIndex = totalCount;
        }
        var result = new PaginationResult<T>();

        //var endIndex = index + totalCount -1;
        var PageCount = (int)Math.Ceiling(totalCount / (double)itemCount);
        if (itemCount == 0)
        {
            PageCount = 0;
        }
        if (endIndex >= totalCount)
        {
            endIndex = totalCount;
        }
        result.TotalCount = totalCount;
        result.Data = await query.Skip(index).Take(itemCount).ToListAsync();
        result.PageCount = PageCount;
        result.HasNextPage = index + itemCount < totalCount;
        result.HasPreviousPage = index > 0;
        result.Start = index + 1;
        result.End = endIndex;
        return result;
    }
}
