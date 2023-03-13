using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace AdeiesApplication.Domain
{
    public class RegistrationRequest
    {
        [Key]
        public int Id { get; set; }
#pragma warning disable CS8618 // Non-nullable property 'Applicant' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public virtual User Applicant { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Applicant' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public int ApplicantId { get; set; }
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
