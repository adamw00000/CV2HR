using Microsoft.EntityFrameworkCore.Migrations;

namespace CV_2_HR.Migrations
{
    public partial class CvFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CvUrl",
                table: "JobApplications");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "JobOffers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CvFileName",
                table: "JobApplications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CvFileName",
                table: "JobApplications");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "JobOffers",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "CvUrl",
                table: "JobApplications",
                nullable: false,
                defaultValue: "");
        }
    }
}
