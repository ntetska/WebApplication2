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
            Vacation toukan = vacation.ToModel(petitioner);
            toukan = await _vacationRepository.AddAsync(toukan);
            if (toukan == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return Ok(toukan);
        }
        [Authorize(Roles = "Manager")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromForm] bool status, int id)
        {
            //if (status == null)
            //    return BadRequest();
            Vacation vacation = await _vacationRepository.GetByIdAsync(id);
            if (vacation == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if (vacation.Status != VacationStatus.Pending)
            {
                return BadRequest("The request is not pending");
            }
            if (status == true)
            {
                vacation.Status = VacationStatus.Accepted;
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
