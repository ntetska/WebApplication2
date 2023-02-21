using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;


namespace WebApplication2.Domain
{
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

        public void ToDTO(UserDTO userDto)
        {
            userDto.Username = Username;
            userDto.Password = Password;
            userDto.FirstName = FirstName;
            userDto.LastName = LastName;
            userDto.Number = Number;
            userDto.Email = Email;
        }
    }

    public class UserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Number { get; set; }
        public string? Email { get; set; }

        public void ToModel(User user)
        {
            user.Username = Username;
            user.Password = Password;
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Number = Number;
            user.Email = Email;
        }
    }

    public enum UserRole
    {
        User,
        Manager,
        Admin
    }
}
