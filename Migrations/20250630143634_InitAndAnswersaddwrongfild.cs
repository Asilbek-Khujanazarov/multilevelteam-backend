using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autotest.Platform.Migrations
{
    /// <inheritdoc />
    public partial class InitAndAnswersaddwrongfild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WrongAnswerCount",
                table: "TestSessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WrongAnswerCount",
                table: "TestSessions");
        }
    }
}
