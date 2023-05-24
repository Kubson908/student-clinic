using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Przychodnia.Webapi.Migrations
{
    /// <inheritdoc />
    public partial class SetPeselAsUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Patients_Pesel",
                table: "Patients",
                column: "Pesel",
                unique: true,
                filter: "[Pesel] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Pesel",
                table: "Employees",
                column: "Pesel",
                unique: true,
                filter: "[Pesel] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_Pesel",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Pesel",
                table: "Employees");
        }
    }
}
