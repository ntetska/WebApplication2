using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Domain {

    public class Vacation
    {
        [Key]
        public int Id { get; set; }
        public User Petitioner { get; set; }
        public int PetitionerId { get; set; }
        public DateTime BeginningDate { get; set; }
        public DateTime EndingDate { get; set; }
        //public int RestOfVacation { get; set; }
        public VacationStatus Status { get; set; } = VacationStatus.Pending;
    }
    public class VacationDto 
    {
        public int PetitionerId { get; set; }
        public DateTime BeginningDate { get; set; }
        public DateTime EndingDate { get; set; }
        public Vacation ToModel(User user) 
        {

            Vacation vacation = new Vacation() 
            { 
                Petitioner = user,
                PetitionerId = PetitionerId,
                BeginningDate = BeginningDate,  
                EndingDate = EndingDate
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
}
