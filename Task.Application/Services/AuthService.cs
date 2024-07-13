using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Clinic.Application.DTOS;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using PaySkyTask.Domain.Entities;
using PaySkyTask.Core.Result;
using PaySkyTask.Core.JWT;
using PaySkyTask.Domain.Enums;
using PaySkyTask.Application.Contracts;

namespace PaySkyTask.Application.Services
{
    public class AuthService(UserManager<User> userManager, ILogger<User> logger, IMapper mapper, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager = null) : IAuthService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly ILogger<User> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly JWT _jwt = jwt.Value;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;


    

        /// <summary>
        /// Logs a user in.
        /// </summary>
        /// <param name="LoginRequestDto">The login model.</param>
        /// <returns>The result of the login attempt.</returns>
        public async Task<Result<LoginResponseDto>> Login(LoginRequestDto LoginRequestDto)
        {
            var user = await _userManager.FindByNameAsync(LoginRequestDto.UserName);

            if (user is null || !await _userManager.CheckPasswordAsync(user, LoginRequestDto.Password))
            {
                _logger.LogWarning($"Invalid credentials entered for {LoginRequestDto.UserName}");
                return Result<LoginResponseDto>.Error("Invalid credentials");
            }

            var roles = await _userManager.GetRolesAsync(user);

            // Map the string roles to UserRole enum
            var mappedRoles = roles
                .Select(role => Enum.TryParse<UserRole>(role, true, out var parsedRole) ? parsedRole : (UserRole?)null)
                .Where(role => role.HasValue)
                .Select(role => role.Value)
                .ToList();

            var token = GenerateJwtToken(user, mappedRoles);

            // Map user to LoginResponseDto
            var loginResponseDto = _mapper.Map<LoginResponseDto>(user);
            loginResponseDto.Roles = mappedRoles;
            loginResponseDto.Token = token;

            _logger.LogInformation($"Successfully generated claims for {user.UserName}");
            _logger.LogInformation($"{user.UserName} successfully logged in");
            return Result<LoginResponseDto>.Success(loginResponseDto);
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="user">The user to register.</param>
        /// <param name="password">The user's entered password.</param>
        /// <returns>The result of the registration attempt.</returns>
        public async Task<Result<User>> RegisterAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }

                _logger.LogError($"An error occurred while creating user: {errors}");
                return Result<User>.Error(errors);
            }

            _logger.LogInformation($"Successfully registered a new user with username {user.UserName}");
            return Result<User>.Success(user);
        }

        /// <summary>
        /// Adds a user to a specific role.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="roleName">The role name.</param>
        /// <returns>The result of trying to add a user to a role.</returns>
        public async Task<Result> AddUserToRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                _logger.LogError($"Unable to find user with id {userId} to assign {roleName} role");
                return Result.NotFound("The user is not found");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                _logger.LogError($"Invalid role name {roleName} while assigning {user.UserName} to the role");
                return Result.Error("Invalid role name");
            }

            _logger.LogInformation($"Successfully assigned {user.UserName} to role {roleName}");
            return Result.SuccessWithMessage($"Successfully assigned {user.FirstName} {user.LastName} to role {roleName}");
        }

        /// <summary>
        /// Registers a user and assigns a role.
        /// </summary>
        /// <param name="user">The user to register.</param>
        /// <param name="password">The user's entered password.</param>
        /// <param name="role">The role to assign to the user.</param>
        /// <returns>The result of the registration attempt.</returns>
        public async Task<Result> RegisterUserWithRoleAsync(User user, string password, string role)
        {
            var registerResult = await RegisterAsync(user, password);

            if (!registerResult.IsSuccess)
            {
                return Result.Error(registerResult.Errors.SingleOrDefault());
            }

            var addRoleResult = await AddUserToRoleAsync(registerResult.Value.Id, role);

            if (!addRoleResult.IsSuccess)
            {
                return addRoleResult;
            }

            return Result.SuccessWithMessage($"Successfully created a new user {registerResult.Value.UserName} with role {role}");
        }

        /// <summary>
        /// Registers a new employer with the employer role.
        /// </summary>
        /// <param name="registerRequestDto">The DTO of the employer.</param>
        /// <returns>The result of the registration attempt.</returns>
        public async Task<Result> RegisterEmployerAsync(RegisterRequestDto registerRequestDto)
        {
            string role = UserRole.Employer.ToString();

            var employer = _mapper.Map<User>(registerRequestDto);

            return await RegisterUserWithRoleAsync(employer, registerRequestDto.Password, role);
        }

        /// <summary>
        /// Registers a new applicant with the applicant role.
        /// </summary>
        /// <param name="registerRequestDto">The DTO of the applicant.</param>
        /// <returns>The result of the registration attempt.</returns>
        public async Task<Result> RegisterApplicantAsync(RegisterRequestDto registerRequestDto)
        {
            string role = UserRole.Applicant.ToString();

            var applicant = _mapper.Map<User>(registerRequestDto);

            return await RegisterUserWithRoleAsync(applicant, registerRequestDto.Password, role);
        }

        /// <summary>
        /// Generates claims for a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roles">The roles.</param>
        /// <returns>The claims identity.</returns>
        public List<Claim> GenerateClaims(User user, List<UserRole> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            // Add roles as claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            return claims;
        }

        /// <summary>
        /// Generates a JWT token for a specific user.
        /// </summary>
        /// <param name="user">The user to generate a JWT token for.</param>
        /// <param name="roles">The user's roles.</param>
        /// <returns>The JWT security token.</returns>
        public string GenerateJwtToken(User user, List<UserRole> roles)
        {
            var claims = GenerateClaims(user, roles);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;
        }

        /// <summary>
        /// Initializes roles.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InitializeRoles()
        {
            var roles = new List<string> { "Employer", "Applicant" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = role });
                }
            }
        }

       
    }
}
