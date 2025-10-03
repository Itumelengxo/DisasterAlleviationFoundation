using System;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class VolunteerTask
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; } = DateTime.UtcNow;

        public DateTime End { get; set; } = DateTime.UtcNow.AddDays(1);

        [StringLength(100)]
        public string RequiredSkills { get; set; }

        // Username of assigned volunteer
        public string AssignedTo { get; set; }

        public bool Completed { get; set; } = false;
    }
}
