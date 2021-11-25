using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp.Infrastructure.Migrations
{
    public partial class InitialMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_User_CreatedByEmail",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectSupervisor_User_SupervisorsEmail",
                table: "ProjectSupervisor");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Projects_ProjectId",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "StudyBankUser");

            migrationBuilder.RenameIndex(
                name: "IX_User_ProjectId",
                table: "StudyBankUser",
                newName: "IX_StudyBankUser_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_User_Email",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_StudyBankUser_ProjectId",
                table: "User",
                newName: "IX_User_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_StudyBankUser_Email",
                table: "User",
                newName: "IX_User_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_User_CreatedByEmail",
                table: "Projects",
                column: "CreatedByEmail",
                principalTable: "User",
                principalColumn: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectSupervisor_User_SupervisorsEmail",
                table: "ProjectSupervisor",
                column: "SupervisorsEmail",
                principalTable: "User",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Projects_ProjectId",
                table: "User",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
