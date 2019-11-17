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
                    Com_Name = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mst_Communication", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mst_Download",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Column_Name = table.Column<string>(nullable: true),
                    Set_Inqury = table.Column<string>(nullable: true),
                    Set_Format = table.Column<string>(nullable: true),
                    Order_No = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mst_Download", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mst_Status",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Status_Name = table.Column<string>(maxLength: 20, nullable: false)
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
                    System_name = table.Column<string>(maxLength: 20, nullable: false)
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
                    Type_Name = table.Column<string>(maxLength: 20, nullable: false)
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
                    Login_Id = table.Column<string>(maxLength: 10, nullable: false),
                    User_Name = table.Column<string>(maxLength: 10, nullable: true),
                    Password = table.Column<string>(maxLength: 12, nullable: false),
                    Hostname = table.Column<string>(nullable: false),
                    DisconnectableFlag = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mst_User", x => new { x.Id, x.Login_Id });
                });

            migrationBuilder.CreateTable(
                name: "Tra_Entry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Company_Name = table.Column<string>(nullable: true),
                    Tan_Name = table.Column<string>(nullable: true),
                    Tel_No = table.Column<string>(nullable: false),
                    Hostname = table.Column<string>(nullable: true),
                    Del_Flag = table.Column<bool>(nullable: false),
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
                    Check_Flag = table.Column<bool>(nullable: false),
                    Company_Name = table.Column<string>(nullable: false),
                    Tan_Name = table.Column<string>(nullable: false),
                    Tel_No = table.Column<string>(nullable: false),
                    Login_Id = table.Column<int>(nullable: false),
                    Inqury = table.Column<string>(nullable: true),
                    Answer = table.Column<string>(nullable: true),
                    Complate_Flag = table.Column<bool>(nullable: false),
                    Start_day = table.Column<DateTime>(nullable: false),
                    Start_Time = table.Column<DateTime>(nullable: false),
                    Fin_Time = table.Column<DateTime>(nullable: false)
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
                name: "Mst_Download");

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
