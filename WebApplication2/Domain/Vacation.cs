
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Domain {

    public class Vacation
    {
        [Key]
        public int Id { get; set; }
        public User Petitioner { get; set; }
        public int PetitionerId { get; set; }
        //public DateTime currentDate { get; set; } = DateTime.Now;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double AnnualLeave { get; set; } = 21;
        public double ParentalLeave { get; set; } = 2;
        public double SickLeave { get; set; } = 12;
        public double StudyLeave { get; set; } = 18;
        public VacationType Type { get; set; } 
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

    [JsonConverter(typeof(StringEnumConverter))]
    public enum VacationStatus
    {
        Pending,
        Accepted,
        Rejected
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum VacationType
    {
        Annual,
        Parental,
        Sick,
        Study
    }
}
