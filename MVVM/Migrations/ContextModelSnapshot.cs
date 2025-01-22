﻿// <auto-generated />
using System;
using MVVM.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MVVM.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MVVM.Models.Customer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("MVVM.Models.Employee", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("MVVM.Models.Executor", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("Executors");
                });

            modelBuilder.Entity("MVVM.Models.Project", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("DateEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateStart")
                        .HasColumnType("datetime2");

                    b.Property<int>("ID_Manager")
                        .HasColumnType("int");

                    b.Property<int>("ID_customer")
                        .HasColumnType("int");

                    b.Property<int>("ID_executor")
                        .HasColumnType("int");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.HasIndex("ID_Manager");

                    b.HasIndex("ID_customer");

                    b.HasIndex("ID_executor");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("MVVM.Models.TeamOfWorker", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("ID_Employee")
                        .HasColumnType("int");

                    b.Property<int>("ID_Project")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ID_Employee");

                    b.HasIndex("ID_Project");

                    b.ToTable("TeamsOfWorkers");
                });

            modelBuilder.Entity("MVVM.Models.Project", b =>
                {
                    b.HasOne("MVVM.Models.Employee", "Manager")
                        .WithMany()
                        .HasForeignKey("ID_Manager")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MVVM.Models.Customer", "Customer")
                        .WithMany("Projects")
                        .HasForeignKey("ID_customer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MVVM.Models.Executor", "Executor")
                        .WithMany("Projects")
                        .HasForeignKey("ID_executor")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Executor");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("MVVM.Models.TeamOfWorker", b =>
                {
                    b.HasOne("MVVM.Models.Employee", "Employee")
                        .WithMany("TeamsOfWorkers")
                        .HasForeignKey("ID_Employee")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MVVM.Models.Project", "Project")
                        .WithMany("TeamsOfWorkers")
                        .HasForeignKey("ID_Project")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("MVVM.Models.Customer", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("MVVM.Models.Employee", b =>
                {
                    b.Navigation("TeamsOfWorkers");
                });

            modelBuilder.Entity("MVVM.Models.Executor", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("MVVM.Models.Project", b =>
                {
                    b.Navigation("TeamsOfWorkers");
                });
#pragma warning restore 612, 618
        }
    }
}