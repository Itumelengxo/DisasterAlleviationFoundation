using System;
using System.ComponentModel.DataAnnotations;

namespace DisasterAlleviationFoundation.Models
{
    public class Donation
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string DonorName { get; set; }

        [StringLength(200)]
        public string ResourceType { get; set; } // Food, Clothing, Medical, Money

        public int Quantity { get; set; } // units, or 1 for monetary

        public decimal? Amount { get; set; } // monetary amount, optional

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string CreatedBy { get; set; } // username
    }
}
