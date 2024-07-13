using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasySkyTask.Application.Contracts;
using PasySkyTask.Application.DTOS;
using PaySkyTask.Core.Result;

namespace PaySkyTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController(IApplicationService applicationService) : ControllerBase
    {
        private readonly IApplicationService _applicationService = applicationService;


        /// <summary>
        /// Gets applications by applicant ID.
        /// </summary>
        /// <param name="applicantId">The ID of the applicant.</param>
        /// <returns>A list of applications for the specified applicant.</returns>
        [HttpGet]
        [Authorize(Roles = "Employer")]
        [ProducesResponseType(typeof(Result<IReadOnlyList<ApplicationResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<Result<IReadOnlyList<ApplicationResponseDto>>> GetApplicationsByApplicant(Guid applicantId)
        {
            return await _applicationService.GetApplicationsByApplicantAsync(applicantId);
        }

        /// <summary>
        /// Applies to a vacancy.
        /// </summary>
        /// <param name="request">The application request DTO.</param>
        /// <returns>The application response DTO.</returns>
        [HttpPost]
        [Authorize(Roles = "Applicant")]
        [ProducesResponseType(typeof(Result<ApplicationResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<Result<ApplicationResponseDto>> ApplyToVacancy(ApplicationRequestDto request)
        {
            return await _applicationService.ApplyToVacancyAsync(request);
        }

        /// <summary>
        /// Gets applicants for a vacancy.
        /// </summary>
        /// <param name="vacancyId">The ID of the vacancy.</param>
        /// <returns>A list of applications for the specified vacancy.</returns>
        [HttpGet("{vacancyId}/applicants")]
        [Authorize(Roles = "Applicant")]
        [ProducesResponseType(typeof(Result<IReadOnlyList<ApplicationResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<Result<IReadOnlyList<ApplicationResponseDto>>> GetApplicantsForVacancy(Guid vacancyId)
        {
            return await _applicationService.GetApplicantsForVacancyAsync(vacancyId);
        }
    }
}
