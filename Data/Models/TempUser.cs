using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KisaanMart.Data.Models
{
    public class TempUser
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        public string UserPhoneNo { get; set; }
        
        [HiddenInput(DisplayValue = false)]
        public double Latitude { get; set; }
        [HiddenInput(DisplayValue = false)]
        public double Longitude { get; set; }
        public bool? IsModerator { get; set; }


        public int? RandOTP { get; set; }
    }
}
