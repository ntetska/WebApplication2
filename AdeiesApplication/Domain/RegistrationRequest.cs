﻿using System.ComponentModel.DataAnnotations;

namespace AdeiesApplication.Domain
{
    public class RegistrationRequest
    {
        [Key]
        public int Id { get; set; }
        public virtual User Applicant { get; set; }
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
