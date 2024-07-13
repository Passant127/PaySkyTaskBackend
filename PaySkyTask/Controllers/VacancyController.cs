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
    public class VacancyController(IVacancyService vacancyService) : ControllerBase
    {
        private readonly IVacancyService _vacancyService = vacancyService;

     
        /// <summary>
        /// Gets all vacancies.
        /// </summary>
        /// <returns>A list of vacancies.</returns>
        [HttpGet]
        [Authorize(Roles = "Employer")]
        [ProducesResponseType(typeof(Result<List<VacancyResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<Result<IReadOnlyList<VacancyResponseDto>>> GetAllVacancies()
        {
            return await _vacancyService.GetAllVacanciesAsync();
        }

        /// <summary>
        /// Retrieves a vacancy by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the vacancy to retrieve.</param>
        /// <returns>A result containing the vacancy response DTO.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Employer")]
        [ProducesResponseType(typeof(Result<VacancyResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<Result<VacancyResponseDto>> GetVacancyById(Guid id)
        {
            return await _vacancyService.GetVacancyByIdAsync(id);
        }

        /// <summary>
        /// Adds a new vacancy asynchronously.
        /// </summary>
        /// <param name="vacancyDto">The DTO representing the vacancy to create.</param>
        /// <returns>A result indicating the outcome of the add operation.</returns>
        [HttpPost]
        [Authorize(Roles = "Employer")]
        [ProducesResponseType(typeof(Result<VacancyResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<Result<VacancyResponseDto>> AddVacancy(VacancyRequestDto vacancyDto)
        {
            return await _vacancyService.AddVacancyAsync(vacancyDto);
        }

        /// <summary>
        /// Updates an existing vacancy by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the vacancy to update.</param>
        /// <param name="vacancyDto">The DTO representing the updated vacancy.</param>
        /// <returns>A result containing the updated vacancy response DTO.</returns>
        [HttpPut]
        [Authorize(Roles = "Employer")]
        [ProducesResponseType(typeof(Result<VacancyResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<Result<VacancyResponseDto>> UpdateVacancy(VacancyRequestDto vacancyDto)
        {
            return await _vacancyService.UpdateVacancyAsync(vacancyDto);
        }

        /// <summary>
        /// Deletes a vacancy by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the vacancy to delete.</param>
        /// <returns>A result indicating the outcome of the deletion operation.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Employer")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<Result<bool>> DeleteVacancy(Guid id)
        {
            return await _vacancyService.DeleteVacancyAsync(id);
        }

        /// <summary>
        /// Archives expired vacancies.
        /// </summary>
        /// <returns>A result indicating the outcome of the archiving operation.</returns>
        [HttpGet("Archive")]
        [Authorize(Roles = "Employer")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public Result<bool> ArchiveExpiredVacancies()
        {
            return _vacancyService.ArchiveExpiredVacanciesAsync();
        }

        /// <summary>
        /// Searches vacancies based on a search term.
        /// </summary>
        /// <param name="search">The search term.</param>
        /// <returns>A list of vacancies matching the search term.</returns>
        [HttpGet("Search/{search}")]
        [Authorize(Roles = "Applicant")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<Result<IReadOnlyList<VacancyResponseDto>>> SearchVacancies(string search)
        {
            return await _vacancyService.SearchVacanciesAsync(search);
        }

        /// <summary>
        /// Posts a vacancy by its ID.
        /// </summary>
        /// <param name="vacancyId">The ID of the vacancy to post.</param>
        /// <returns>A result indicating the outcome of the post operation.</returns>
        [HttpPut("PostVacancy/{vacancyId}")]
        [Authorize(Roles = "Employer")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<Result<bool>> PostVacancy(Guid vacancyId)
        {
            return await _vacancyService.PostVacancyAsync(vacancyId);
        }

        /// <summary>
        /// Deactivates a vacancy by its ID.
        /// </summary>
        /// <param name="vacancyId">The ID of the vacancy to deactivate.</param>
        /// <returns>A result indicating the outcome of the deactivation operation.</returns>
        [HttpPut("DeactivateVacancy/{vacancyId}")]
        [Authorize(Roles = "Employer")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<Result<bool>> DeactivateVacancy(Guid vacancyId)
        {
            return await _vacancyService.DeactivateVacancyAsync(vacancyId);
        }
    }
}
