using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autotest.Platform.Migrations
{
    /// <inheritdoc />
    public partial class InitQuestsixdonssssAndAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "TestSessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "TestSessions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "TestSessions");

            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "TestSessions");
        }
    }
}
