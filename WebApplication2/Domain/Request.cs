using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace WebApplication2.Domain
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        public User Applicant { get; set; }
        public int ApplicantId { get; set; }
        //public string Username { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public RequestCondition Condition { get; set; } = RequestCondition.Pending;
    }

    public enum RequestCondition
    {
        Pending,
        Accepted,
        Rejected
    }
}
