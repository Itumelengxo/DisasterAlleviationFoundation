using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class UserModel
    {
        // Unique username (used as key in memory)
        [Required, StringLength(100)]
        public string Username { get; set; }

        // Hashed password storage (not raw)
        public string PasswordHash { get; set; }

        // Salt used in hashing
        public string PasswordSalt { get; set; }

        // Role (User or Admin)
        public string Role { get; set; } = "User";

        // Profile fields
        [StringLength(200)]
        public string FullName { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }
    }
}
