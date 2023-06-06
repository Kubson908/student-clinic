using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Przychodnia.Webapi.Migrations
{
    /// <inheritdoc />
    public partial class addDiagnosisToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Diagnosis",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Diagnosis",
                table: "Appointments");
        }
    }
}
