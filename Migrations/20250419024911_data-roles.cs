using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIBILA_API.Migrations
{
    /// <inheritdoc />
    public partial class dataroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Roles (Nombre) VALUES ('Beneficiario')");
            migrationBuilder.Sql("INSERT INTO Roles (Nombre) VALUES ('Personal administrativo')");
            migrationBuilder.Sql("INSERT INTO Roles (Nombre) VALUES ('Administrador')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
