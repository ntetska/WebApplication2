using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;
using WebApplication2.Domain;
using WebApplication2.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication2.Controllers.Api
{
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVacation(int id)
        {
            var vacation = await _vacationRepository.GetByIdAsync(id);
            return Ok(vacation);
        }
        [Authorize(Roles = "Manager")]
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
            //check if the user insert sat or sun date
            for (DateTime date = vacation.StartDate; date <= vacation.EndDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
                {
                    throw new ArgumentException("start date or end date is weeknend day");
                }
            }
            //check validity of days
            if (vacation.EndDate < vacation.StartDate)
                throw new ArgumentException("endDate must be greater than or equal to startDate");
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
                    DateTime d1 = vacation.StartDate;
                    DateTime d2 = vacation.EndDate;

                    TimeSpan ts = d2 - d1;

                    double totalDays = ts.TotalDays;

                    vacation.AnnualLeave = (int)(vacation.AnnualLeave - totalDays);
                    vacation = await _vacationRepository.UpdateAsync(vacation);
                }

                if (isVacactionType && typeRes == VacationType.Parental)
                {
                    vacation.Type = VacationType.Parental;
                    DateTime d1 = vacation.StartDate;
                    DateTime d2 = vacation.EndDate;

                    TimeSpan ts = d2 - d1;

                    double totalDays = ts.TotalDays;

                    vacation.ParentalLeave = (int)(vacation.ParentalLeave - totalDays);
                    vacation = await _vacationRepository.UpdateAsync(vacation);
                }
                if (isVacactionType && typeRes == VacationType.Study)
                {
                    vacation.Type = VacationType.Study;
                    DateTime d1 = vacation.StartDate;
                    DateTime d2 = vacation.EndDate;

                    TimeSpan ts = d2 - d1;

                    double totalDays = ts.TotalDays;

                    vacation.StudyLeave = (int)(vacation.StudyLeave - totalDays);
                    vacation = await _vacationRepository.UpdateAsync(vacation);

                }
                if (isVacactionType && typeRes == VacationType.Sick)
                {
                    vacation.Type = VacationType.Sick;
                    DateTime d1 = vacation.StartDate;
                    DateTime d2 = vacation.EndDate;

                    TimeSpan ts = d2 - d1;

                    double totalDays = ts.TotalDays;

                    vacation.SickLeave = (int)(vacation.SickLeave - totalDays);
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
            List<Vacation> DeletedVacations = new List<Vacation>();
            Vacation vacation = await _vacationRepository.DeleteAsync(id);
            DeletedVacations.Add(vacation);
            if (vacation == null)
            {
                return BadRequest();
            }
            return Ok(vacation);
        }
    }
}
