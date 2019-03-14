using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DeborahCall.Migrations
{
    public partial class Deborah : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mst_Communication",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Com_Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mst_Communication", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mst_Status",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Status_Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mst_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mst_System",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    System_name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mst_System", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mst_Type",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Type_Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mst_Type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mst_User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    User_Name = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Hostname = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mst_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tra_Entry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Tel_No = table.Column<string>(nullable: true),
                    Hostname = table.Column<string>(nullable: true),
                    Entry_Time = table.Column<DateTime>(nullable: false),
                    End_Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tra_Entry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tra_Inqury",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Entry_Id = table.Column<int>(nullable: false),
                    System_Id = table.Column<int>(nullable: false),
                    Com_Id = table.Column<int>(nullable: false),
                    Type_Id = table.Column<int>(nullable: false),
                    Relation_Id = table.Column<int>(nullable: false),
                    Staff_Flag = table.Column<bool>(nullable: false),
                    Company_Name = table.Column<string>(nullable: true),
                    Tan_Name = table.Column<string>(nullable: true),
                    Tel_No = table.Column<string>(nullable: true),
                    Inqury = table.Column<long>(nullable: false),
                    Answer = table.Column<long>(nullable: false),
                    Cause = table.Column<long>(nullable: false),
                    Incomplate_Flag = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tra_Inqury", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mst_Communication");

            migrationBuilder.DropTable(
                name: "Mst_Status");

            migrationBuilder.DropTable(
                name: "Mst_System");

            migrationBuilder.DropTable(
                name: "Mst_Type");

            migrationBuilder.DropTable(
                name: "Mst_User");

            migrationBuilder.DropTable(
                name: "Tra_Entry");

            migrationBuilder.DropTable(
                name: "Tra_Inqury");
        }
    }
}
