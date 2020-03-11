using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KisaanMart.Data.Models
{
    public class Machine
    {
        public int Id { get; set; }
        [Display(Name ="Machine")]
        public string MachineName { get; set; }
        [Display(Name = "Description")]

        [DataType(DataType.MultilineText)]
        public string MachineDescription { get; set; }

        [Display(Name="Image")]
        public byte[] MachineImage { get; set; }

    }
}
