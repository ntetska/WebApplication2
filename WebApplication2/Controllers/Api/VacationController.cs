using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using WebApplication2.Domain;
using WebApplication2.Services;

namespace WebApplication2.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationController : ControllerBase
    {
        private IRepository<User> _userRepository;
        private IRepository<Vacation> _vacationRepository;

        public VacationController(IRepository<Vacation> vacationRepository,IRepository<User> userRepository)
        {
            _userRepository=userRepository;
            _vacationRepository = vacationRepository;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            var vacation = await _vacationRepository.GetByIdAsync(id);
            return Ok(vacation);

        }
        [Authorize(Roles = "Manager")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var vacation = await _vacationRepository.GetAllAsync();
            return Ok(vacation);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(VacationDto vacation)
        {
            User petitioner = await _userRepository.GetByIdAsync(vacation.PetitionerId);
            Vacation vacationRequest = vacation.ToModel(petitioner);
            vacationRequest = await _vacationRepository.AddAsync(vacationRequest);
            if (vacationRequest == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return Ok(vacationRequest);
        }
        [Authorize(Roles = "Manager")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromForm] bool status, int id)
        {
            
            Vacation vacation = await _vacationRepository.GetByIdAsync(id);
            if (vacation == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            if (vacation.Status != VacationStatus.Pending)
            {
                return BadRequest("The request is not pending");
            }

            if (status)
            {
                vacation.Status = VacationStatus.Accepted;
                //int TotalDays = 
            }

            if (!status)
            {
                vacation.Status = VacationStatus.Rejected;
            }

            vacation = await _vacationRepository.UpdateAsync(vacation);
            return Ok(vacation);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Vacation vacation = await _vacationRepository.DeleteAsync(id);
            if (vacation == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return Ok(vacation);
        }
    }
}
