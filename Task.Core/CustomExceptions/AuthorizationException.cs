using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.CustomExceptions;

[Serializable]
public class AuthorizationException : Exception
{
    public AuthorizationException()
        : base("Authorization error")
    {
    }

    public AuthorizationException(string message)
        : base(message)
    {
    }

    public AuthorizationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
