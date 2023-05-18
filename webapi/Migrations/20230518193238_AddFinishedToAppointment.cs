using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Przychodnia.Webapi.Migrations
{
    /// <inheritdoc />
    public partial class AddFinishedToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Finished",
                table: "Appointments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Finished",
                table: "Appointments");
        }
    }
}
