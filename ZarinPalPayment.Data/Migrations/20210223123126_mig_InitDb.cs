using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZarinPalPayment.Data.Migrations
{
    public partial class mig_InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    RequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    PaymentAmount = table.Column<long>(type: "bigint", nullable: false),
                    PaymentDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestStatus = table.Column<int>(type: "int", nullable: false),
                    ResponseCode = table.Column<int>(type: "int", nullable: false),
                    ResponseAuthority = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceID = table.Column<int>(type: "int", nullable: false),
                    RequestDatetime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.RequestID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requests");
        }
    }
}
