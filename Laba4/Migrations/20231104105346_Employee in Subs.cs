using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laba4.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeinSubs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Subscriptions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_EmployeeId",
                table: "Subscriptions",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Employees_EmployeeId",
                table: "Subscriptions",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Employees_EmployeeId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_EmployeeId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Subscriptions");
        }
    }
}
