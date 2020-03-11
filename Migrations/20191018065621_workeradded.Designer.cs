﻿// <auto-generated />
using System;
using KisaanMart.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KisaanMart.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20191018065621_workeradded")]
    partial class workeradded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("KisaanMart.Data.Models.Apps", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppDescription");

                    b.Property<byte[]>("AppImage");

                    b.Property<string>("AppName");

                    b.Property<string>("AppUrl");

                    b.HasKey("Id");

                    b.ToTable("Apps");
                });

            modelBuilder.Entity("KisaanMart.Data.Models.Machine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("MachineDescription");

                    b.Property<byte[]>("MachineImage");

                    b.Property<string>("MachineName");

                    b.HasKey("Id");

                    b.ToTable("Machines");
                });

            modelBuilder.Entity("KisaanMart.Data.Models.TempUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("IsModerator");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<int?>("RandOTP");

                    b.Property<string>("UserName");

                    b.Property<string>("UserPassword");

                    b.Property<string>("UserPhoneNo")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("TempUsers");
                });

            modelBuilder.Entity("KisaanMart.Data.Models.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndDate");

                    b.Property<int>("NoOfAcres");

                    b.Property<int>("NoOfHours");

                    b.Property<DateTime>("StartDate");

                    b.Property<double>("TotalAmountToBePaid");

                    b.Property<int>("UserMachineId");

                    b.Property<int>("behalfuserId");

                    b.Property<int>("requesteduserId");

                    b.HasKey("Id");

                    b.HasIndex("UserMachineId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("KisaanMart.Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("IsActive");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<int>("ModeratorId");

                    b.Property<int>("PointsAccumulated");

                    b.Property<string>("Role");

                    b.Property<string>("UserName");

                    b.Property<string>("UserPassword");

                    b.Property<string>("UserPhoneNo")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("KisaanMart.Data.Models.UserMachine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("AmountPerAcre");

                    b.Property<double>("AmountPerHour");

                    b.Property<int>("MachineId");

                    b.Property<int>("userId");

                    b.HasKey("Id");

                    b.HasIndex("MachineId");

                    b.HasIndex("userId");

                    b.ToTable("UserMachines");
                });

            modelBuilder.Entity("KisaanMart.Data.Models.Worker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.Property<string>("UserPhoneNo")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Workers");
                });

            modelBuilder.Entity("KisaanMart.ViewModels.LoginViewModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Role");

                    b.Property<string>("UserPassword");

                    b.Property<string>("UserPhoneNo");

                    b.HasKey("Id");

                    b.ToTable("LoginViewModel");
                });

            modelBuilder.Entity("KisaanMart.ViewModels.UserMachineViewModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("AmountPerAcre");

                    b.Property<double>("AmountPerHour");

                    b.Property<int?>("SelectedMachineId")
                        .IsRequired();

                    b.Property<int?>("userId");

                    b.HasKey("Id");

                    b.ToTable("UserMachineViewModel");
                });

            modelBuilder.Entity("KisaanMart.Data.Models.Transaction", b =>
                {
                    b.HasOne("KisaanMart.Data.Models.UserMachine", "UserMachines")
                        .WithMany()
                        .HasForeignKey("UserMachineId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KisaanMart.Data.Models.UserMachine", b =>
                {
                    b.HasOne("KisaanMart.Data.Models.Machine", "Machines")
                        .WithMany()
                        .HasForeignKey("MachineId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KisaanMart.Data.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
