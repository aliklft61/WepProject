using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ItServiceApp.Models.Identity;

namespace ItServiceApp.Models.Entities
{
    public class Address : BaseEntity
    {
        public string Line { get; set; }
        public string PostCode { get; set; }
        public AddressTypes AddresType { get; set; }
        
        public int  StateId { get; set; }

        [StringLength(450)]
        public string UserId { get; set; }

        

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

    }

    public enum AddressTypes
    {
        Fatura,
        Teslimat
    }
}
