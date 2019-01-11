using Microsoft.EntityFrameworkCore.Migrations;

namespace CV2HR.Migrations
{
    public partial class CreateSearchIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "JobTitle",
                table: "JobOffers",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_JobTitle",
                table: "JobOffers",
                column: "JobTitle");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JobOffers_JobTitle",
                table: "JobOffers");

            migrationBuilder.AlterColumn<string>(
                name: "JobTitle",
                table: "JobOffers",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
