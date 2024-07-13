using Clinic.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySkyTask.Application.Services;

public class UserContextService:IUserContextService
{
    public string UserId { get; set; } = "";
    public string Email { get; set; } = "";
}
