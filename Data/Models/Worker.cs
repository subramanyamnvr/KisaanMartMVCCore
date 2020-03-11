using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KisaanMart.Data.Models
{
    public class Worker
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Person Name")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Phone No")]
        public string UserPhoneNo { get; set; }
    }
}
