
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Domain {

    public class Vacation
    {
        [Key]
        public int Id { get; set; }
        public User Petitioner { get; set; }
        public int PetitionerId { get; set; }
        public DateTime currentDate { get; set; } = DateTime.Now;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double VacationDays { get; set; } = 21;
        public VacationType Type { get; set; } = VacationType.RegularLeave;
        public VacationStatus Status { get; set; } = VacationStatus.Pending;
    }
    public class VacationDto
    {
        public int PetitionerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Vacation ToModel(User user)
        {

            Vacation vacation = new Vacation()
            {
                Petitioner = user,
                PetitionerId = PetitionerId,
                StartDate = StartDate,
                EndDate = EndDate
            };
            return vacation;
        }
    }
    public enum VacationStatus
    {
        Pending,
        Accepted,
        Rejected
    }
    public enum VacationType
    {
        RegularLeave,
        MaternityLeave,
        UnpaidLeave
    }
}
