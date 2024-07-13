using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySkyTask.Core.CustomExceptions;

[Serializable]
public class ResourceNotFoundException : Exception
{
    public string ResourceName { get; }
    public int Id { get; }

    public ResourceNotFoundException() { }

    public ResourceNotFoundException(string resourceName, int id)
    : base(resourceName)
    {
        ResourceName = resourceName;
        Id = id;
    }

    public ResourceNotFoundException(string message, Exception inner)
    : base(message, inner) { }
}