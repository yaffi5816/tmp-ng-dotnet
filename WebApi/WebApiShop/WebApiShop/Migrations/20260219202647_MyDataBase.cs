using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiShop.Migrations
{
    /// <inheritdoc />
    public partial class MyDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__19093A2B9C2D38AF", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CCACAF403EFD", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    TemplateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    TemplateName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    TemplateDescreption = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ImgUrl = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Template__F87ADD07FE218519", x => x.TemplateID);
                    table.ForeignKey(
                        name: "FK__Templates__Categ__3C69FB99",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID");
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ProjectSum = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    TemplateID = table.Column<int>(type: "int", nullable: false),
                    CurrentStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Projects__761ABED060DA2D86", x => x.ProjectID);
                    table.ForeignKey(
                        name: "FK__Projects__Templa__44FF419A",
                        column: x => x.TemplateID,
                        principalTable: "Templates",
                        principalColumn: "TemplateID");
                    table.ForeignKey(
                        name: "FK__Projects__UserID__440B1D61",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Categori__8517B2E0E0111658",
                table: "Categories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_TemplateID",
                table: "Projects",
                column: "TemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UserID",
                table: "Projects",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_CategoryID",
                table: "Templates",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "UQ__Template__1BCAF4FCFFBDD523",
                table: "Templates",
                column: "ImgUrl",
                unique: true,
                filter: "[ImgUrl] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__Template__A6C2DA667CD2D337",
                table: "Templates",
                column: "TemplateName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Users__C9F284561F8CF10D",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
