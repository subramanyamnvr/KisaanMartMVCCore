using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KisaanMart.Data.Models
{
    public class UserMachine
    {
        public int Id { get; set; }

        [Required]    
        [Display(Name="Amount Per Hour in Rupees")]
        public double AmountPerHour { get; set; }
        [Display(Name = "Amount Per Acre in Rupees")]
        public double AmountPerAcre { get; set; }

        public int Distance { get; set; }

        [Display(Name = "User")]
        public int userId { get; set; }
        [ForeignKey("userId")]
        public User user { get; set; }


        [Display(Name = "Machine")]
        [Required]
        public int MachineId { get; set; }

        [ForeignKey("MachineId")]
        public virtual Machine Machines { get; set; }
        //public Machine machine { get; set; }

    }
}
