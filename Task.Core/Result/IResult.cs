using System;
using System.Collections.Generic;


namespace PaySkyTask.Core.Result;

public interface IResult
{
    ResultStatus Status { get; }
    IEnumerable<string> Errors { get; }
    List<ValidationError> ValidationErrors { get; }
    Type ValueType { get; }
    Object GetValue();
}
