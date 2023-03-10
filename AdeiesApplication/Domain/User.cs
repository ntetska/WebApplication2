using System.ComponentModel.DataAnnotations;

namespace AdeiesApplication.Domain
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
        public User? Manager { get; set; }
        public List<User> ManagedUsers { get; set; } = new List<User>();
        //public string ManagerUsername { get; set; }
        public int VacationDays { get; set; } = 21;
        public RegistrationRequest? Request { get; set; }
        public List<Vacation> Vacation { get; set; } = new List<Vacation>();
        public bool IsActive { get; set; } = false;

        public UserDTO ToDTO()
        {
            return new UserDTO
            {
                Id = Id,
                Username = Username,
                FirstName = FirstName,
                LastName = LastName,
                Number = Number,
                Email = Email,
                Role = Role
            };
        }
        public UserCreate ToCreate()
        {
            return new UserCreate
            {
                Username = Username,
                Password = Password,
                FirstName = FirstName,
                LastName = LastName,
                Number = Number,
                Email = Email,
            };
        }
    }
    public class UserCreate
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string Number { get; set; }
        public string? Email { get; set; }
        public void ToModelCreate(User user)
        {
            user.Username = Username;
            user.Password = Password;
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Number = Number;
            user.Email = Email;

        }
    }
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Number { get; set; }
        public string? Email { get; set; }
        public UserRole Role { get; set; }

        public void ToModel(User user)
        {
            user.Id = Id;
            user.Username = Username;
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Number = Number;
            user.Email = Email;
            user.Role = Role;
        }
    }

    public enum UserRole
    {
        User,
        Manager,
        Admin
    }
}
