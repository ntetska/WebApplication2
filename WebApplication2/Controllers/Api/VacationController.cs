using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication2.Domain;
using WebApplication2.Services;


namespace WebApplication2.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VacationController : ControllerBase
    {
        private IRepository<User> _userRepository;
        private IRepository<Vacation> _vacationRepository;

        public double TotalDays { get; private set; }

        public VacationController(IRepository<Vacation> vacationRepository, IRepository<User> userRepository)
        {
            _userRepository = userRepository;
            _vacationRepository = vacationRepository;
        }
        [HttpGet("GetVacation/{id}")]
        public async Task<IActionResult> GetVacation(int id)
        {
            var vacation = await _vacationRepository.GetByIdAsync(id);
            var userCookieID = HttpContext.User.FindFirstValue("Id");
            if (userCookieID != vacation.PetitionerId.ToString())
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Unathorized user");
            }
            return Ok(vacation);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var vacations = await _vacationRepository.GetAllAsync();
            return Ok(vacations);
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            var vacation = await _vacationRepository.GetByIdAsync(id);
            return Ok(vacation);
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("ManagedVac")]
        public async Task<IActionResult> GetAllAsync()
        {
            List<Vacation> ManagedVac = new List<Vacation>();

            var userCookieID = HttpContext.User.FindFirstValue("Id");

            List<Vacation> vacations = await _vacationRepository.GetAllAsync();

            foreach (var vacation in vacations)
            {
                if (vacation.Petitioner.Manager != null && (vacation.Petitioner.Manager.Id == (int.Parse(userCookieID))))
                {
                    ManagedVac.Add(vacation);
                }
            }
            return Ok(ManagedVac);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(VacationDto vacation)
        {
            var userCookieID = HttpContext.User.FindFirstValue("Id");
            User petitioner = await _userRepository.GetByIdAsync(int.Parse(userCookieID));
            if (userCookieID != vacation.PetitionerId.ToString())
            {
                return Forbid("Forbidden");
            }
            Vacation vacationRequest = vacation.ToModel(petitioner);
            //check if the user insert sat or sun date
            for (DateOnly date = vacation.StartDate; date <= vacation.EndDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
                {
                    return BadRequest("start date or end date is weeknend day");
                }
            }
            //check validity of days
            if (vacation.EndDate < vacation.StartDate)
            {
                return BadRequest("endDate must be greater than or equal to startDate");
            }
            if (petitioner.Role == UserRole.Manager)
            {
                vacationRequest.Status = VacationStatus.Accepted;
            }
            vacationRequest = await _vacationRepository.AddAsync(vacationRequest);
            return Ok(vacationRequest);
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromForm] string status,[FromForm] string type, int id)
        {
            Vacation vacation = await _vacationRepository.GetByIdAsync(id);
            VacationStatus statusRes;
            bool isVacactionStatus = Enum.TryParse(status, true, out statusRes);

            VacationType typeRes;
            bool isVacactionType = Enum.TryParse(type, true, out typeRes);
            var userCookieID = HttpContext.User.FindFirstValue("Id");
            if (vacation.Petitioner.Manager != null && (userCookieID != vacation.Petitioner.Manager.Id.ToString()))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Forbidden");
            }
            var user = await _userRepository.GetByIdAsync(int.Parse(userCookieID));

            if (vacation == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            if (vacation.Status != VacationStatus.Pending)
            {
                return BadRequest("The request is not pending");
            }
            //if approved, type gets a value
            if (isVacactionStatus && statusRes == VacationStatus.Accepted)
            {
                vacation.Status = VacationStatus.Accepted;
                if (isVacactionType && typeRes == VacationType.Annual)
                {
                    vacation.Type = VacationType.Annual;

                    DateTime d1 = vacation.StartDate.Date;
                    DateTime d2 = vacation.EndDate.Date;

                    TimeSpan ts = d2 - d1;

                    int totalDays = ts.Days;

                    vacation.AnnualLeave = vacation.AnnualLeave - totalDays;
                    vacation = await _vacationRepository.UpdateAsync(vacation);
                }

                if (isVacactionType && typeRes == VacationType.Parental)
                {
                    vacation.Type = VacationType.Parental;
                    DateTime d1 = vacation.StartDate.Date;
                    DateTime d2 = vacation.EndDate.Date;

                    TimeSpan ts = d2 - d1;

                    int totalDays = ts.Days;

                    vacation.ParentalLeave = vacation.ParentalLeave - totalDays;
                    vacation = await _vacationRepository.UpdateAsync(vacation);
                }
                if (isVacactionType && typeRes == VacationType.Study)
                {
                    vacation.Type = VacationType.Study;
                    DateTime d1 = vacation.StartDate.Date;
                    DateTime d2 = vacation.EndDate.Date;

                    TimeSpan ts = d2 - d1;

                    int totalDays = ts.Days;

                    vacation.StudyLeave = vacation.StudyLeave - totalDays;
                    vacation = await _vacationRepository.UpdateAsync(vacation);

                }
                if (isVacactionType && typeRes == VacationType.Sick)
                {
                    vacation.Type = VacationType.Sick;
                    DateTime d1 = vacation.StartDate.Date;
                    DateTime d2 = vacation.EndDate.Date;

                    TimeSpan ts = d2 - d1;

                    int totalDays = ts.Days;

                    vacation.SickLeave = vacation.SickLeave - totalDays;
                    vacation = await _vacationRepository.UpdateAsync(vacation);
                }
                vacation = await _vacationRepository.UpdateAsync(vacation);
            }

            List<Vacation> vacations = new List<Vacation>();
            if (isVacactionStatus && statusRes == VacationStatus.Rejected)
            {
                vacations.Add(vacation);
            }
            vacation = await _vacationRepository.UpdateAsync(vacation);
            return Ok(vacation);
        }

        //user can delete only his vacation
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vacation = await _vacationRepository.GetByIdAsync(id);
            var userCookieID = HttpContext.User.FindFirstValue("Id");
            if (userCookieID != vacation.PetitionerId.ToString())
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Unathorized user");
            }
            List<Vacation> DeletedVacations = new List<Vacation>();
            Vacation delVacation = await _vacationRepository.DeleteAsync(id);
            DeletedVacations.Add(delVacation);
            if (vacation == null)
            {
                return BadRequest();
            }
            return Ok(vacation);
        }
    }
}
