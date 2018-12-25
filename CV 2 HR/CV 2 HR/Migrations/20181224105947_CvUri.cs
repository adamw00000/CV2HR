using Microsoft.EntityFrameworkCore.Migrations;

namespace CV_2_HR.Migrations
{
    public partial class CvUri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CvFileName",
                table: "JobApplications",
                newName: "CvUri");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CvUri",
                table: "JobApplications",
                newName: "CvFileName");
        }
    }
}
