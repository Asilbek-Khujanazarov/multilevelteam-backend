using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autotest.Platform.Migrations
{
    /// <inheritdoc />
    public partial class InqitAndAnswersaddwrongfild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WrongAnswerCount",
                table: "TestSessions");

            migrationBuilder.AddColumn<string>(
                name: "FailureReason",
                table: "TestSessions",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailureReason",
                table: "TestSessions");

            migrationBuilder.AddColumn<int>(
                name: "WrongAnswerCount",
                table: "TestSessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
