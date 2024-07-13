using AutoMapper;
using Microsoft.Extensions.Logging;
using PasySkyTask.Application.Contracts;
using PasySkyTask.Application.DTOS;
using PasySkyTask.Domain.IRepositories;
using PaySkyTask.Core.Result;
using PaySkyTask.Domain.Entities;

namespace PasySkyTask.Application.Services
{

    public class ApplicationService(IApplicationRepository applicationRepository, IMapper mapper, ILogger<ApplicationService> logger) : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository = applicationRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<ApplicationService> _logger = logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationService"/> class.
        /// </summary>
        /// <param name="applicationRepository">The application repository.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        /// <param name="logger">The logger instance.</param>
       

        /// <summary>
        /// Gets applications by applicant ID.
        /// </summary>
        /// <param name="applicantId">The applicant ID.</param>
        /// <returns>A list of applications for the specified applicant.</returns>
        public async Task<Result<IReadOnlyList<ApplicationResponseDto>>> GetApplicationsByApplicantAsync(Guid applicantId)
        {
            var applications = await _applicationRepository.GetApplicationsByApplicantAsync(applicantId);

            if (applications == null || applications.Count == 0)
            {
                _logger.LogWarning("No applications found for applicant ID {ApplicantId}", applicantId);
                return Result<IReadOnlyList<ApplicationResponseDto>>.NotFound("No applications found.");
            }

            var applicationDtos = _mapper.Map<List<ApplicationResponseDto>>(applications);
            return Result<IReadOnlyList<ApplicationResponseDto>>.Success(applicationDtos);
        }

        /// <summary>
        /// Gets an application by its ID.
        /// </summary>
        /// <param name="applicationId">The application ID.</param>
        /// <returns>The application details.</returns>
        public async Task<Result<ApplicationResponseDto>> GetApplicationByIdAsync(Guid applicationId)
        {
            var application = await _applicationRepository.GetByIdAsync(applicationId);

            if (application == null)
            {
                _logger.LogWarning("Application with ID {ApplicationId} not found", applicationId);
                return Result<ApplicationResponseDto>.NotFound("Application not found.");
            }

            var applicationDto = _mapper.Map<ApplicationResponseDto>(application);
            return Result<ApplicationResponseDto>.Success(applicationDto);
        }

        /// <summary>
        /// Applies to a vacancy.
        /// </summary>
        /// <param name="request">The application request DTO.</param>
        /// <returns>The application response DTO.</returns>
        public async Task<Result<ApplicationResponseDto>> ApplyToVacancyAsync(ApplicationRequestDto request)
        {
            if (request == null)
            {
                _logger.LogWarning("Invalid request object for applying to vacancy.");
                return Result<ApplicationResponseDto>.NotFound("Invalid request object.");
            }

            var canApplyResult = await CanApplyToVacancyAsync(request.VacancyId, request.ApplicantId);
            if (!canApplyResult.IsSuccess || !canApplyResult.Value)
            {
                _logger.LogWarning("Applicant cannot apply to vacancy. Reason: {Reason}", canApplyResult.Errors);
                return Result<ApplicationResponseDto>.NotFound("Cannot apply to vacancy.");
            }

            var applicationDto = _mapper.Map<Applications>(request);
            var savedApplication = await _applicationRepository.AddAsync(applicationDto);

            var applicationResponse = _mapper.Map<ApplicationResponseDto>(request);
            return Result<ApplicationResponseDto>.Success(applicationResponse);
        }

        /// <summary>
        /// Checks if an applicant can apply to a vacancy.
        /// </summary>
        /// <param name="vacancyId">The vacancy ID.</param>
        /// <param name="applicantId">The applicant ID.</param>
        /// <returns>A boolean result indicating if the applicant can apply.</returns>
        public async Task<Result<bool>> CanApplyToVacancyAsync(Guid vacancyId, Guid applicantId)
        {
            var canApply = await _applicationRepository.CheckEligibilityAsync(vacancyId, applicantId);
            var hasAppliedWithin24Hours = await _applicationRepository.HasAppliedWithin24HoursAsync(applicantId);
            return Result<bool>.Success(canApply && !hasAppliedWithin24Hours);
        }

        /// <summary>
        /// Gets applicants for a vacancy.
        /// </summary>
        /// <param name="vacancyId">The vacancy ID.</param>
        /// <returns>A list of applicants for the specified vacancy.</returns>
        public async Task<Result<IReadOnlyList<ApplicationResponseDto>>> GetApplicantsForVacancyAsync(Guid vacancyId)
        {
            _logger.LogInformation("Getting applicants for vacancy ID: {VacancyId}", vacancyId);

            var applications = await _applicationRepository.GetApplicationsByVacancyIdAsync(vacancyId);

            _logger.LogInformation("Found {Count} applicants for vacancy ID: {VacancyId}", applications.Count, vacancyId);

            var mappedResult = _mapper.Map<IReadOnlyList<ApplicationResponseDto>>(applications);
            return Result<IReadOnlyList<ApplicationResponseDto>>.Success(mappedResult);
        }
    }
}
