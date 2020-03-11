using KisaanMart.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KisaanMart.ViewModels;

namespace KisaanMart.Data
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        public ApplicationContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=IN-ATS2444\SQLEXPRESS02;Database=KisaanDB;Trusted_Connection=True;");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<UserMachine> UserMachines { get; set; }

        public DbSet<TempUser> TempUsers { get; set; }

        public DbSet<Apps> Apps { get; set; }

        public DbSet<Worker> Workers { get; set; }


        public DbSet<KisaanMart.ViewModels.LoginViewModel> LoginViewModel { get; set; }

        public DbSet<KisaanMart.ViewModels.UserMachineViewModel> UserMachineViewModel { get; set; }

        


    }
}
