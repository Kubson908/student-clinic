using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Przychodnia.Webapi.Migrations
{
    /// <inheritdoc />
    public partial class AddedVerifiedFieldToPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Verified",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Verified",
                table: "Patients");
        }
    }
}
