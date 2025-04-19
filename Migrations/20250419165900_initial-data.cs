using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIBILA_API.Migrations
{
    /// <inheritdoc />
    public partial class initialdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Roles (Nombre) VALUES ('Beneficiario')");
            migrationBuilder.Sql("INSERT INTO Roles (Nombre) VALUES ('Personal administrativo')");
            migrationBuilder.Sql("INSERT INTO Roles (Nombre) VALUES ('Administrador')");

            // Insertar 10 usuarios
            migrationBuilder.Sql(@"
                INSERT INTO Usuarios (Nombre, Apellido, TipoDocumento, Documento, CorreoElectronico, Contrasena, RolId)
                VALUES
                ('admin', 'admin', 1, '1128123456', 'admin@sibila.com', '12345', 3),
                ('Sofía', 'García', 1, '28765432A', 'sofia.garcia@email.com', 'SofiaPass123!', 1),
                ('Carlos', 'Martínez', 1, '37654321B', 'carlos.martinez@email.com', 'CarlosSecure456!', 1),
                ('Elena', 'Rodríguez', 1, '46543218C', 'elena.rodriguez@email.com', 'Elena789$', 1),
                ('Javier', 'López', 1, '55432187D', 'javier.lopez@email.com', 'JavierPass321#', 1),
                ('Lucía', 'Hernández', 1, '64321876E', 'lucia.hernandez@email.com', 'Lucia654@', 1),
                ('Miguel', 'Gómez', 1, '73218765F', 'miguel.gomez@email.com', 'Miguel987!', 1),
                ('Ana', 'Pérez', 1, '82187654G', 'ana.perez@email.com', 'AnaPass246%', 1),
                ('David', 'Sánchez', 1, '91876543H', 'david.sanchez@email.com', 'David357$', 1),
                ('Laura', 'Díaz', 1, '00765432I', 'laura.diaz@email.com', 'Laura864#', 1),
                ('Daniel', 'Fernández', 1, '09654321J', 'daniel.fernandez@email.com', 'Daniel159!', 1)
            ");

            // Insertar 20 libros
            migrationBuilder.Sql(@"
                INSERT INTO Libros (Titulo, Autor, Editorial, ISBN, Subcategoria, TipoMaterial, Estado)
                VALUES
                ('Cien años de soledad', 'Gabriel García Márquez', 'Penguin Random House', '978-0307474728', 'Realismo mágico', 'Novela', 0),
                ('1984', 'George Orwell', 'Secker & Warburg', '978-0451524935', 'Distopía', 'Novela', 0),
                ('El nombre del viento', 'Patrick Rothfuss', 'DeBolsillo', '978-8401352836', 'Fantasía épica', 'Novela', 0),
                ('Sapiens', 'Yuval Noah Harari', 'Debate', '978-8499926223', 'Historia humana', 'Ensayo', 0),
                ('Orgullo y prejuicio', 'Jane Austen', 'Alianza Editorial', '978-8420664550', 'Romance clásico', 'Novela', 0),
                ('El principito', 'Antoine de Saint-Exupéry', 'Salamandra', '978-8498381498', 'Literatura infantil', 'Fábula', 0),
                ('El código Da Vinci', 'Dan Brown', 'Umbriel', '978-8408093853', 'Thriller histórico', 'Novela', 0),
                ('La sombra del viento', 'Carlos Ruiz Zafón', 'Planeta', '978-8408043643', 'Misterio gótico', 'Novela', 0),
                ('Breves respuestas a grandes preguntas', 'Stephen Hawking', 'Crítica', '978-8491990440', 'Divulgación científica', 'Ensayo', 0),
                ('Harry Potter y la piedra filosofal', 'J.K. Rowling', 'Salamandra', '978-8498381405', 'Fantasía juvenil', 'Novela', 0),
                ('Crónica de una muerte anunciada', 'Gabriel García Márquez', 'Penguin Random House', '978-0307474729', 'Realismo mágico', 'Novela', 0),
                ('Rebelión en la granja', 'George Orwell', 'Secker & Warburg', '978-0451524936', 'Sátira política', 'Novela', 0),
                ('El temor de un hombre sabio', 'Patrick Rothfuss', 'DeBolsillo', '978-8401352837', 'Fantasía épica', 'Novela', 0),
                ('Homo Deus', 'Yuval Noah Harari', 'Debate', '978-8499926224', 'Futuro de la humanidad', 'Ensayo', 0),
                ('Emma', 'Jane Austen', 'Alianza Editorial', '978-8420664551', 'Romance clásico', 'Novela', 0),
                ('El arte de la guerra', 'Sun Tzu', 'Penguin Clásicos', '978-8491052011', 'Estrategia militar', 'Tratado', 0),
                ('Los pilares de la Tierra', 'Ken Follett', 'Plaza & Janés', '978-8401337209', 'Novela histórica', 'Novela', 0),
                ('El alquimista', 'Paulo Coelho', 'Planeta', '978-8408146765', 'Ficción espiritual', 'Novela', 0),
                ('El señor de los anillos', 'J.R.R. Tolkien', 'Minotauro', '978-8445000667', 'Fantasía heroica', 'Novela', 0),
                ('El retrato de Dorian Gray', 'Oscar Wilde', 'Alianza Editorial', '978-8420664552', 'Clásico gótico', 'Novela', 0)
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar los datos insertados
            migrationBuilder.Sql("DELETE FROM Usuarios WHERE Documento IN ('28765432A', '37654321B', '46543218C', '55432187D', '64321876E', '73218765F', '82187654G', '91876543H', '00765432I', '09654321J')");
            migrationBuilder.Sql("DELETE FROM Libros WHERE ISBN IN ('978-0307474728', '978-0451524935', '978-8401352836', '978-8499926223', '978-8420664550', '978-8498381498', '978-8408093853', '978-8408043643', '978-8491990440', '978-8498381405', '978-0307474729', '978-0451524936', '978-8401352837', '978-8499926224', '978-8420664551', '978-8491052011', '978-8401337209', '978-8408146765', '978-8445000667', '978-8420664552')");
        }
    }
}
