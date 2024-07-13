using PasySkyTask.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Contracts;

public interface IUserContextService: ILifeTime, IScopedService
{
    public string UserId { get; set; }
    public string Email { get; set; }
}
