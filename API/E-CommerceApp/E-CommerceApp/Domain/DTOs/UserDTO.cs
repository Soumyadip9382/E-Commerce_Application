using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace E_CommerceApp.Domain.DTOs
{
    public class UserDTO
    {
        [NotMapped]
        public class CreateUser
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            [EmailAddress]
            public string Email { get; set; }
            public string Password { get; set; }
            [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone must be 10 digits")]
            public string Phone { get; set; }
        }

        [NotMapped]
        public class UpdateUser
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
        }


        [NotMapped]
        public class Loggedin
        {
            public string PhoneOrEmail { get; set; }
            public string Password { get; set; }
        }

        [NotMapped]
        public class UpdatePassword
        {
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmNewPassword { get; set; }
        }
    }
    
}
