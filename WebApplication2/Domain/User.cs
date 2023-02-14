using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;


namespace WebApplication2.Domain
{
    //[Index(nameof(Username),IsUnique = true)]

    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Number { get; set; }
        public string? Email { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
        public User? Manager { get; set; }
        public IEnumerable<User> ManagedUsers { get; set; } = Enumerable.Empty<User>();
        public string? ManagerUsername { get; set; }
        public RegistrationRequest? Request { get; set; }
        public bool IsActive { get; set; } = false;
    }

    public enum UserRole
    {
        User,
        Manager,
        Admin
    }
}
