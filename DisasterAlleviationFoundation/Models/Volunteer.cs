using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class Volunteer
    {
        public int Id { get; set; } // Unique identifier

        [Required, StringLength(50)]
        public string Username { get; set; } // For login

        [Required, StringLength(100)]
        public string Password { get; set; } // Plain-text for in-memory auth

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(200)]
        public string Skills { get; set; }

        [StringLength(200)]
        public string Availability { get; set; }

        [StringLength(200)]
        public string EmergencyContact { get; set; }

        [StringLength(300)]
        public string Address { get; set; }

        public int HoursContributed { get; set; } = 0;

        // List of task IDs assigned to this volunteer (in-memory)
        public List<int> AssignedTaskIds { get; set; } = new List<int>();
    }
}
