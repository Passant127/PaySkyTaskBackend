using Clinic.Application.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasySkyTask.Application.DTOS;
using PaySkyTask.Application.Contracts;
using PaySkyTask.Core.Result;

namespace Clinic.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService= authService;

    /// <summary>
    /// action for registration a new patient that take patient request dto.
    /// </summary>
    /// <param name="registerModel">The registration model.</param>
    /// <returns>result representing of the registration successfully.</returns>
    [HttpPost("RegisterApplicant")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<Result> RegisterAsync(RegisterRequestDto patientDto)
    {

        var result = await _authService.RegisterApplicantAsync(patientDto);

        return result;
    }


    /// <summary>
    /// action for registration a new patient that take patient request dto.
    /// </summary>
    /// <param name="registerModel">The registration model.</param>
    /// <returns>result representing of the registration successfully.</returns>
    [HttpPost("RegisterEmployer")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<Result> RegisterEmployeerAsync(RegisterRequestDto userRequest)
    {

        var result = await _authService.RegisterEmployerAsync(userRequest);

        return result;
    }

    /// <summary>
    /// action for login a user that take login request dto.
    /// </summary>
    /// <param name="loginModel">The login model.</param>
    /// <returns>result representing of the login successfully.</returns>
    [HttpPost("Login")]
    [ProducesResponseType(typeof(Result<LoginResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginModel)
    {
        var result = await _authService.Login(loginModel);

        if (!result.IsSuccess)
        {
            return result;
        }

        return result;
    }


    /// <summary>
    /// action for add a staff roles.
    /// </summary>
    /// <returns>result representing the adding the staff roles successfully.</returns>
    [HttpPost("CreateInitialRoles")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<Result> CreateRoles()
    {
        await _authService.InitializeRoles();

         return Result.SuccessWithMessage("Create roles successfully");
    }
  
}

