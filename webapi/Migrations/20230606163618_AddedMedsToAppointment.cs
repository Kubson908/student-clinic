using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Przychodnia.Webapi.Migrations
{
    /// <inheritdoc />
    public partial class AddedMedsToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Meds",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Meds",
                table: "Appointments");
        }
    }
}
