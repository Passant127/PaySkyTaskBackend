
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Clinic.Application.Contracts;
using Clinic.Application.DTOS;
using PasySkyTask.Application.Contracts;
using PasySkyTask.Application.DTOS;
using PaySkyTask.Core.Result;

namespace PaySkyTask.Application.Contracts;

public interface IAuthService:  IScopedService , ILifeTime
{
    Task<Result<LoginResponseDto>> Login(LoginRequestDto LoginRequestDto);
    Task<Result> RegisterEmployerAsync(RegisterRequestDto userRequest);
    Task<Result> RegisterApplicantAsync(RegisterRequestDto userRequest);
    Task<Result> AddUserToRoleAsync(string userId, string roleName);
    Task InitializeRoles();
   
}
