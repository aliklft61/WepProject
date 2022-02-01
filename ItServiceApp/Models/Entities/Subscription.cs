using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ItServiceApp.Models.Identity;

namespace ItServiceApp.Models.Entities
{
    public class Subscription:BaseEntity
    {
        public Guid SubscriptionTypeId { get; set; }

        public decimal Amount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime EndDate { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }


        [NotMapped]
        public bool IsActive => EndDate > DateTime.Now;


        [ForeignKey(nameof(SubscriptionTypeId))]
        public virtual SubscriptionType SubscriptionType { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
    }

    public class SubscriptionType : BaseEntity
    {
        [Required,StringLength(50)]
        public string Name { get; set; }

        public int Month { get; set; }
        
        public string Description { get; set; }

    }
}
