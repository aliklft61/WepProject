using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ItServiceApp.Models.Entities
{
    public class BaseEntity
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();
        [StringLength(128)]
        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        [StringLength(128)]
        public string UpdatedUser { get; set; } 

    }
}
