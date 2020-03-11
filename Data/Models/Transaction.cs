using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KisaanMart.Data.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Purpose")]

        public string Purpose { get; set; }

        [Required]
        [Display(Name ="No of Hours")]
        public int NoOfHours { get; set; }

        [Display(Name = "No of Acres")]
        public int NoOfAcres { get; set; }
        [Display(Name = "Total amount to be paid")]
        public double TotalAmountToBePaid { get; set; }

      
        [Display(Name = "UserMachineId")]
        [Required]
        public int UserMachineId { get; set; }

        [ForeignKey("UserMachineId")]
        public virtual UserMachine UserMachines { get; set; }

        [Display(Name = "Requested By")]
        public int requesteduserId { get; set; }

        [Display(Name = "Request On Behalf Of")]
        public int? behalfuserId { get; set; }
        

    }
}
