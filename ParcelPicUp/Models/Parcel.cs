using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace parcelPicUp.Models
{
    public class Parcel
    {
        public int Id { get; set; }

        [Required]
        public string TrackingNumber { get; set; }

        [Required]
        public string SenderId { get; set; }

        public string? ReceiverId { get; set; }

        public string? DeliveryAgentId { get; set; }

        [Required]
        public string ReceiverName { get; set; }

        [Required]
        public string ReceiverAddress { get; set; }

        [Required]
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties (optional for foreign key relationships)
        [ForeignKey("SenderId")]
        public virtual ApplicationUser Sender { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual ApplicationUser Receiver { get; set; }

        [ForeignKey("DeliveryAgentId")]
        public virtual ApplicationUser DeliveryAgent { get; set; }
    }
}
