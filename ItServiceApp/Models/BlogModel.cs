using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ItServiceApp.Models
{
    public class BlogModel
    {
        public int BlogID { get; set; }

        // [Display(Name="Name",Prompt="Enter product name")]
        // [Required(ErrorMessage="Name zorunlu bir alan.")]
        // [StringLength(60,MinimumLength=5,ErrorMessage="Ürün ismi 5-10 karakter aralığında olmalıdır.")]
        public string BlogName { get; set; }

        [Required(ErrorMessage = "Url zorunlu bir alan.")]
        public string BlogUrl { get; set; }

        [Required(ErrorMessage = "Description zorunlu bir alan.")]
        [StringLength(100000, MinimumLength = 5, ErrorMessage = "Description 5-100 karakter aralığında olmalıdır.")]

        public string BlogDescription { get; set; }

        [Required(ErrorMessage = "ImageUrl zorunlu bir alan.")]
        public string BlogImageUrl { get; set; }
        public bool IsHome { get; set; }
    }
}
