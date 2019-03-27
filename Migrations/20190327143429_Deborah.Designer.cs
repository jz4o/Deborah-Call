﻿// <auto-generated />
using System;
using Deborah.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DeborahCall.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20190327143429_Deborah")]
    partial class Deborah
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Deborah.Models.Mst_Communication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Com_Name")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("Mst_Communication");
                });

            modelBuilder.Entity("Deborah.Models.Mst_Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Status_Name")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("Mst_Status");
                });

            modelBuilder.Entity("Deborah.Models.Mst_System", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("System_name")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("Mst_System");
                });

            modelBuilder.Entity("Deborah.Models.Mst_Type", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Type_Name")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("Mst_Type");
                });

            modelBuilder.Entity("Deborah.Models.Mst_User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Hostname")
                        .IsRequired();

                    b.Property<string>("Login_Id")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("User_Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Mst_User");
                });

            modelBuilder.Entity("Deborah.Models.Tra_Entry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Company_Name");

                    b.Property<DateTime>("End_Time");

                    b.Property<DateTime>("Entry_Time");

                    b.Property<string>("Hostname");

                    b.Property<string>("Tan_Name");

                    b.Property<string>("Tel_No")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Tra_Entry");
                });

            modelBuilder.Entity("Deborah.Models.Tra_Inqury", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer");

                    b.Property<int>("Com_Id");

                    b.Property<string>("Company_Name");

                    b.Property<bool>("Complate_Flag");

                    b.Property<int>("Entry_Id");

                    b.Property<DateTime>("Fin_Time");

                    b.Property<string>("Inqury");

                    b.Property<int>("Login_Id");

                    b.Property<int>("Relation_Id");

                    b.Property<bool>("Staff_Flag");

                    b.Property<DateTime>("Start_Time");

                    b.Property<DateTime>("Start_day");

                    b.Property<int>("System_Id");

                    b.Property<string>("Tan_Name");

                    b.Property<string>("Tel_No");

                    b.Property<int>("Type_Id");

                    b.HasKey("Id");

                    b.ToTable("Tra_Inqury");
                });
#pragma warning restore 612, 618
        }
    }
}