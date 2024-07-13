using PaySkyTask.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOS;

public class LoginResponseDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public List<UserRole> Roles { get; set; }
    public string Token { get; set; }
}
