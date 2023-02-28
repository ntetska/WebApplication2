using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly DateTime StartDate;
        private readonly DateTime EndDate;

        public double TotalDays { get; private set; }

        public VacationController(IRepository<Vacation> vacationRepository, IRepository<User> userRepository)
        {
            _userRepository = userRepository;
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
        public async Task<IActionResult> Create(VacationDto vacation, DateTime startDate, DateTime endDate)
        {
            
            User petitioner = await _userRepository.GetByIdAsync(vacation.PetitionerId);
            Vacation vacationRequest = vacation.ToModel(petitioner);
            vacationRequest = await _vacationRepository.AddAsync(vacationRequest);
            //var dates = new List<DateTime>();
            if (endDate < startDate)
                throw new ArgumentException("endDate must be greater than or equal to startDate");

            return Ok(vacationRequest);
        }


        [Authorize(Roles = "Manager")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromForm] bool status, [FromForm] bool type, int id)
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
                DateTime d1 = StartDate;
                DateTime d2 = EndDate;

                TimeSpan ts = d2-d1;
                double days = Math.Abs(ts.Days);
                Vacation vacationDays = vacationDays - days;
            }
            if (!status)
            {
                vacation.Status = VacationStatus.Rejected;
            }
            if (type)
            {
                vacation.Type = VacationType.MaternityLeave;
            }
            if (!type)
            {
                vacation.Type = VacationType.UnpaidLeave;
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
