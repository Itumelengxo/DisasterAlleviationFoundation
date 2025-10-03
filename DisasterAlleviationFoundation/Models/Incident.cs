using System;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class Incident
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Type { get; set; } // e.g., Flood, Fire, Storm

        [Required, StringLength(200)]
        public string Location { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;

        public string ReportedBy { get; set; } // username of reporter
    }
}
