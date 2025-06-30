using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autotest.Platform.Migrations
{
    /// <inheritdoc />
    public partial class IdnitAndAnswersaddwrongfild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailureReason",
                table: "TestSessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FailureReason",
                table: "TestSessions",
                type: "text",
                nullable: true);
        }
    }
}
