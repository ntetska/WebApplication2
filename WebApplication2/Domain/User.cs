using System.ComponentModel.DataAnnotations;

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
        public DateTime HireDate { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
        public User? managerId { get; set; }
        public List<User> ManagedUsers { get; set; } = new List<User>();
        //public string ManagerUsername { get; set; }
        public int VacationDays { get; set; } = 21;
        public RegistrationRequest? Request { get; set; }
        public List<Vacation> Vacation { get; set; } = new List<Vacation>();
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

        public void ToDto(UserDisplay userDisplay)
        {
            userDisplay.Username = Username;
            userDisplay.Number = Number;
        }
    }

    public class UserDisplay
    {
        public string Username { get; set; }
        public string Number { get; set; }

        public void ToModel(User user)
        {
            user.Username = Username;
            user.Number = Number;
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
