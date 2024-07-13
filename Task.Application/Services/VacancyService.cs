using AutoMapper;
using Microsoft.Extensions.Logging;
using PasySkyTask.Application.Contracts;
using PasySkyTask.Application.DTOS;
using PasySkyTask.Domain.IRepositories;
using PaySkyTask.Core.Result;
using PaySkyTask.Domain.Entities;
using PaySkyTask.Domain.Enums;

namespace PasySkyTask.Application.Services
{
    
    public class VacancyService(IVacancyRepository vacancyRepository, IMapper mapper, ILogger<VacancyService> logger) : IVacancyService
    {
        private readonly IVacancyRepository _vacancyRepository = vacancyRepository;
        private readonly ILogger<VacancyService> _logger = logger;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Gets a vacancy by its ID.
        /// </summary>
        /// <param name="vacancyId">The ID of the vacancy.</param>
        /// <returns>A result containing the vacancy DTO if found.</returns>
        public async Task<Result<VacancyResponseDto>> GetVacancyByIdAsync(Guid vacancyId)
        {
            var result = await _vacancyRepository.GetByIdAsync(vacancyId);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to retrieve vacancy with ID {VacancyId}: {Errors}", vacancyId, result.Errors);
                return Result<VacancyResponseDto>.NotFound();
            }

            var vacancyDto = _mapper.Map<VacancyResponseDto>(result.Value);
            return Result<VacancyResponseDto>.Success(vacancyDto);
        }

        /// <summary>
        /// Gets all vacancies.
        /// </summary>
        /// <returns>A result containing a list of all vacancy DTOs.</returns>
        public async Task<Result<IReadOnlyList<VacancyResponseDto>>> GetAllVacanciesAsync()
        {
            var result = await _vacancyRepository.GetAllAsync();

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to retrieve all vacancies: {Errors}", result.Errors);
                return Result<IReadOnlyList<VacancyResponseDto>>.NotFound();
            }

            var vacancyDtos = _mapper.Map<IEnumerable<VacancyResponseDto>>(result.Value);
            return Result<IReadOnlyList<VacancyResponseDto>>.Success(vacancyDtos.ToList());
        }

        /// <summary>
        /// Adds a new vacancy.
        /// </summary>
        /// <param name="vacancy">The vacancy request DTO.</param>
        /// <returns>A result containing the added vacancy DTO.</returns>
        public async Task<Result<VacancyResponseDto>> AddVacancyAsync(VacancyRequestDto vacancy)
        {
            var entity = _mapper.Map<Vacancy>(vacancy);
            var result = await _vacancyRepository.AddAsync(entity);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to add vacancy: {Errors}", result.Errors);
                return Result<VacancyResponseDto>.NotFound();
            }

            var addedVacancyDto = _mapper.Map<VacancyResponseDto>(result.Value);
            return Result<VacancyResponseDto>.Success(addedVacancyDto);
        }

        /// <summary>
        /// Updates an existing vacancy.
        /// </summary>
        /// <param name="vacancy">The vacancy request DTO.</param>
        /// <returns>A result containing the updated vacancy DTO.</returns>
        public async Task<Result<VacancyResponseDto>> UpdateVacancyAsync(VacancyRequestDto vacancy)
        {
            var entity = _mapper.Map<Vacancy>(vacancy);
            var result = await _vacancyRepository.UpdateAsync(entity);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to update vacancy: {Errors}", result.Errors);
                return Result<VacancyResponseDto>.NotFound();
            }

            var updatedVacancyDto = _mapper.Map<VacancyResponseDto>(result.Value);
            return Result<VacancyResponseDto>.Success(updatedVacancyDto);
        }

        /// <summary>
        /// Deletes a vacancy by its ID.
        /// </summary>
        /// <param name="vacancyId">The ID of the vacancy.</param>
        /// <returns>A result indicating success or failure.</returns>
        public async Task<Result<bool>> DeleteVacancyAsync(Guid vacancyId)
        {
            var result = await _vacancyRepository.DeleteAsync(vacancyId);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to delete vacancy with ID {VacancyId}: {Errors}", vacancyId, result.Errors);
                return Result<bool>.NotFound();
            }

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Archives expired vacancies.
        /// </summary>
        /// <returns>A result indicating success or failure.</returns>
        public Result<bool> ArchiveExpiredVacanciesAsync()
        {
            var result = Result<bool>.Success(true);
            var expiredVacancies = _vacancyRepository.GetExpiredVacancies();

            // Check if there are any expired vacancies to process
            if (expiredVacancies.Any())
            {
                foreach (var vacancy in expiredVacancies)
                {
                    try
                    {
                        vacancy.IsArchived = true;
                        _vacancyRepository.UpdateAsync(vacancy);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error archiving vacancy with ID {vacancy.Id}");
                        result = Result<bool>.NotFound($"Error archiving vacancy with ID {vacancy.Id}");
                    }
                }
            }
            else
            {
                result = Result<bool>.NotFound("No expired vacancies found to archive");
            }

            return result;
        }

        /// <summary>
        /// Searches for vacancies based on a search term.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>A result containing a list of matching vacancy DTOs.</returns>
        public async Task<Result<IReadOnlyList<VacancyResponseDto>>> SearchVacanciesAsync(string searchTerm)
        {
            var result = await _vacancyRepository.SearchVacanciesAsync(searchTerm);

            if (result == null)
            {
                _logger.LogError($"Failed to retrieve vacancies for search term: {searchTerm}");
                return Result<IReadOnlyList<VacancyResponseDto>>.NotFound("Failed to search vacancies.");
            }

            var vacancyDtos = _mapper.Map<IEnumerable<VacancyResponseDto>>(result.Value);
            return Result<IReadOnlyList<VacancyResponseDto>>.Success(vacancyDtos.ToList());
        }

        /// <summary>
        /// Posts a vacancy, changing its status to active.
        /// </summary>
        /// <param name="vacancyId">The ID of the vacancy.</param>
        /// <returns>A result indicating success or failure.</returns>
        public async Task<Result<bool>> PostVacancyAsync(Guid vacancyId)
        {
            var vacancy = await _vacancyRepository.GetByIdAsync(vacancyId);
            if (vacancy == null)
            {
                return Result<bool>.NotFound($"Vacancy with ID {vacancyId} not found.");
            }

            vacancy.Value.Status = VacancyStatus.Active;
            await _vacancyRepository.UpdateAsync(vacancy);

            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Deactivates a vacancy, changing its status to inactive.
        /// </summary>
        /// <param name="vacancyId">The ID of the vacancy.</param>
        /// <returns>A result indicating success or failure.</returns>
        public async Task<Result<bool>> DeactivateVacancyAsync(Guid vacancyId)
        {
            var vacancy = await _vacancyRepository.GetByIdAsync(vacancyId);
            if (vacancy == null)
            {
                return Result<bool>.NotFound($"Vacancy with ID {vacancyId} not found.");
            }

            vacancy.Value.Status = VacancyStatus.Inactive;
            await _vacancyRepository.UpdateAsync(vacancy);

            return Result<bool>.Success(true);
        }

    
    }
}
