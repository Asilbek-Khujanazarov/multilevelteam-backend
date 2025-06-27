
// src/API/DTOs/Auth/UserResponse.cs
using Autotest.Platform.Domain.Enums;

namespace Autotest.Platform.API.DTOs.Auth
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public Gender Gender { get; set; }
        public bool IsVerified { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}