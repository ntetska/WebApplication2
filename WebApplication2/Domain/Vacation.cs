using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;


namespace WebApplication2.Domain {

    public class Vacation
    {
        [Key]
        public int Id { get; set; }
        public virtual User Petitioner { get; set; }
        public int PetitionerId { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; } 
        public double AnnualLeave { get; set; } = 21;
        public double ParentalLeave { get; set; } = 2;
        public double SickLeave { get; set; } = 12;
        public double StudyLeave { get; set; } = 18;
        public VacationType Type { get; set; } 
        public VacationStatus Status { get; set; } = VacationStatus.Pending;

        public VacationDto ToDto()
        {
            return new VacationDto
            {
                PetitionerId = PetitionerId,
                StartDate = DateOnly.FromDateTime(StartDate),
                EndDate = DateOnly.FromDateTime(EndDate)
            };
        }
    }



    public class VacationDto
    {
        public int PetitionerId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Vacation ToModel(User user)
        {
            Vacation vacation = new Vacation()
            {
                Petitioner = user,
                PetitionerId = PetitionerId,
                StartDate = StartDate.ToDateTime(new TimeOnly(0,0)),
                EndDate = EndDate.ToDateTime(new TimeOnly(0,0)),
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
