using KisaanMart.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KisaanMart.ViewModels
{
    public class UserMachineViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Amount Per Hour in Rupees")]
        public double AmountPerHour { get; set; }
        [Display(Name = "Amount Per Acre in Rupees")]
        public double AmountPerAcre { get; set; }

        [Required]
        public int? SelectedMachineId { get; set; }
        [NotMapped]
        public SelectList Machines { get; set; }

        public int? userId { get; set; }
        
    }
}
