using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySkyTask.Core.Entities;

public class PaginationResult<T>
{
    public List<T> Data { get; set; }
    public int TotalCount { get; set; }
    public int PageCount { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
}
