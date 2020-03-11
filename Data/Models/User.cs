using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KisaanMart.Data.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Display(Name="User Name")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "User Phone No")]        
        public string UserPhoneNo { get; set; }
        
        [HiddenInput(DisplayValue = false)]
        public bool? IsActive { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string Role { get; set; }
        [HiddenInput(DisplayValue = false)]
        public double Latitude { get; set; }
        [HiddenInput(DisplayValue = false)]
        public double Longitude { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int ModeratorId { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int PointsAccumulated { get; set; }
        public int? RandOTP { get; set; }
    }
}
