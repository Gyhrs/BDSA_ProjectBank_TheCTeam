using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp.Infrastructure.Migrations
{
    public partial class InitialMigrationNikoline5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_StudyBankUser_CreatedByEmail",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectSupervisor_StudyBankUser_SupervisorsEmail",
                table: "ProjectSupervisor");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyBankUser_Projects_ProjectId",
                table: "StudyBankUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudyBankUser",
                table: "StudyBankUser");

            migrationBuilder.RenameTable(
                name: "StudyBankUser",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_StudyBankUser_ProjectId",
                table: "Users",
                newName: "IX_Users_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_StudyBankUser_Email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_CreatedByEmail",
                table: "Projects",
                column: "CreatedByEmail",
                principalTable: "Users",
                principalColumn: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectSupervisor_Users_SupervisorsEmail",
                table: "ProjectSupervisor",
                column: "SupervisorsEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Projects_ProjectId",
                table: "Users",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_CreatedByEmail",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectSupervisor_Users_SupervisorsEmail",
                table: "ProjectSupervisor");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Projects_ProjectId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "StudyBankUser");

            migrationBuilder.RenameIndex(
                name: "IX_Users_ProjectId",
                table: "StudyBankUser",
                newName: "IX_StudyBankUser_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "StudyBankUser",
                newName: "IX_StudyBankUser_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudyBankUser",
                table: "StudyBankUser",
                column: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_StudyBankUser_CreatedByEmail",
                table: "Projects",
                column: "CreatedByEmail",
                principalTable: "StudyBankUser",
                principalColumn: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectSupervisor_StudyBankUser_SupervisorsEmail",
                table: "ProjectSupervisor",
                column: "SupervisorsEmail",
                principalTable: "StudyBankUser",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyBankUser_Projects_ProjectId",
                table: "StudyBankUser",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
