using Microsoft.EntityFrameworkCore;
using SIBILA_API.Models;
using SIBILA_API.Models.Enums;

namespace SIBILA_API.Data
{      
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

            // 1. Crear roles si no existen
            CreateRoles(context);

            // 2. Crear usuarios si no existen
            CreateUsers(context);

            // 3. Crear libros si no existen
            CreateBooks(context);
        }

        private static void CreateRoles(AppDbContext context)
        {
            if (!context.Roles.Any())
            {
                context.Database.ExecuteSqlRaw("INSERT INTO Roles (Nombre) VALUES ('Beneficiario')");
                context.Database.ExecuteSqlRaw("INSERT INTO Roles (Nombre) VALUES ('Personal administrativo')");
                context.Database.ExecuteSqlRaw("INSERT INTO Roles (Nombre) VALUES ('Administrador')");

                context.SaveChanges();
                Console.WriteLine("Roles creados exitosamente");
            }
        }

        private static void CreateUsers(AppDbContext context)
        {
            if (!context.Usuarios.Any())
            {
                // Obtener IDs de roles (asumiendo que se crearon en el orden dado)
                var rolBeneficiarioId = context.Roles.First(r => r.Nombre == "Beneficiario").Id;
                var rolPersonalId = context.Roles.First(r => r.Nombre == "Personal administrativo").Id;
                var rolAdminId = context.Roles.First(r => r.Nombre == "Administrador").Id;

                var usuarios = new[]
                {
                    new Usuarios {
                        Nombre = "admin",
                        Apellido = "admin",
                        TipoDocumento = TipoDocumentoUsuarioEnum.CC,
                        Documento = "1128123456",
                        CorreoElectronico = "admin@sibila.com",
                        Contrasena = "12345",
                        RolId = rolAdminId
                    },
                    new Usuarios {
                        Nombre = "María",
                        Apellido = "García",
                        TipoDocumento = TipoDocumentoUsuarioEnum.CC,
                        Documento = "11223344A",
                        CorreoElectronico = "maria.garcia@email.com",
                        Contrasena = "MariaG2024!",
                        RolId = rolBeneficiarioId
                    },
                    new Usuarios {
                        Nombre = "Carlos",
                        Apellido = "Rodríguez",
                        TipoDocumento = TipoDocumentoUsuarioEnum.CC,
                        Documento = "22334455B",
                        CorreoElectronico = "carlos.rodriguez@email.com",
                        Contrasena = "CarlosR#123",
                        RolId = rolBeneficiarioId
                    },
                    new Usuarios {
                        Nombre = "Ana",
                        Apellido = "López",
                        TipoDocumento = TipoDocumentoUsuarioEnum.CC,
                        Documento = "33445566C",
                        CorreoElectronico = "ana.lopez@email.com",
                        Contrasena = "AnaLopez$456",
                        RolId = rolBeneficiarioId
                    },
                    new Usuarios {
                        Nombre = "Javier",
                        Apellido = "Martínez",
                        TipoDocumento = TipoDocumentoUsuarioEnum.CC,
                        Documento = "44556677D",
                        CorreoElectronico = "javier.martinez@email.com",
                        Contrasena = "JavierM@789",
                        RolId = rolBeneficiarioId
                    },
                    new Usuarios {
                        Nombre = "Lucía",
                        Apellido = "Hernández",
                        TipoDocumento = TipoDocumentoUsuarioEnum.CC,
                        Documento = "55667788E",
                        CorreoElectronico = "lucia.hernandez@email.com",
                        Contrasena = "LuciaH2024!",
                        RolId = rolBeneficiarioId
                    },
                    new Usuarios {
                        Nombre = "Pedro",
                        Apellido = "Gómez",
                        TipoDocumento = TipoDocumentoUsuarioEnum.CC,
                        Documento = "66778899F",
                        CorreoElectronico = "pedro.gomez@email.com",
                        Contrasena = "PedroG#321",
                        RolId = rolBeneficiarioId
                    },
                    new Usuarios {
                        Nombre = "Sofía",
                        Apellido = "Díaz",
                        TipoDocumento = TipoDocumentoUsuarioEnum.CC,
                        Documento = "77889900G",
                        CorreoElectronico = "sofia.diaz@email.com",
                        Contrasena = "SofiaD$654",
                        RolId = rolBeneficiarioId
                    },
                    new Usuarios {
                        Nombre = "Daniel",
                        Apellido = "Pérez",
                        TipoDocumento = TipoDocumentoUsuarioEnum.CC,
                        Documento = "88990011H",
                        CorreoElectronico = "daniel.perez@email.com",
                        Contrasena = "DanielP@987",
                        RolId = rolBeneficiarioId
                    },
                    new Usuarios {
                        Nombre = "Elena",
                        Apellido = "Sánchez",
                        TipoDocumento = TipoDocumentoUsuarioEnum.CC,
                        Documento = "99001122I",
                        CorreoElectronico = "elena.sanchez@email.com",
                        Contrasena = "ElenaS2024!",
                        RolId = rolBeneficiarioId
                    },
                    new Usuarios {
                        Nombre = "Miguel",
                        Apellido = "Fernández",
                        TipoDocumento = TipoDocumentoUsuarioEnum.CC,
                        Documento = "00112233J",
                        CorreoElectronico = "miguel.fernandez@email.com",
                        Contrasena = "MiguelF#159",
                        RolId = rolBeneficiarioId
                    }
                };

                context.Usuarios.AddRange(usuarios);
                context.SaveChanges();
                Console.WriteLine("Usuarios creados exitosamente");
            }
        }

        private static void CreateBooks(AppDbContext context)
        {
            if (!context.Libros.Any())
            {
                var libros = new[]
                {
                new Libro {
                    Titulo = "Cien años de soledad",
                    Autor = "Gabriel García Márquez",
                    Editorial = "Penguin Random House",
                    ISBN = "978-0307474728",
                    Subcategoria = "Realismo mágico",
                    TipoMaterial = "Novela",
                    Estado = 0
                },
                new Libro {
                    Titulo = "1984",
                    Autor = "George Orwell",
                    Editorial = "Secker & Warburg",
                    ISBN = "978-0451524935",
                    Subcategoria = "Distopía",
                    TipoMaterial = "Novela",
                    Estado = 0
                },
                new Libro {
                    Titulo = "El nombre del viento",
                    Autor = "Patrick Rothfuss",
                    Editorial = "DeBolsillo",
                    ISBN = "978-8401352836",
                    Subcategoria = "Fantasía épica",
                    TipoMaterial = "Novela",
                    Estado = 0
                },
                new Libro {
                    Titulo = "Sapiens",
                    Autor = "Yuval Noah Harari",
                    Editorial = "Debate",
                    ISBN = "978-8499926223",
                    Subcategoria = "Historia humana",
                    TipoMaterial = "Ensayo",
                    Estado = 0
                },
                new Libro {
                    Titulo = "Orgullo y prejuicio",
                    Autor = "Jane Austen",
                    Editorial = "Alianza Editorial",
                    ISBN = "978-8420664550",
                    Subcategoria = "Romance clásico",
                    TipoMaterial = "Novela",
                    Estado = 0
                },
                new Libro {
                    Titulo = "El principito",
                    Autor = "Antoine de Saint-Exupéry",
                    Editorial = "Salamandra",
                    ISBN = "978-8498381498",
                    Subcategoria = "Literatura infantil",
                    TipoMaterial = "Fábula",
                    Estado = 0
                },
                new Libro {
                    Titulo = "El código Da Vinci",
                    Autor = "Dan Brown",
                    Editorial = "Umbriel",
                    ISBN = "978-8408093853",
                    Subcategoria = "Thriller histórico",
                    TipoMaterial = "Novela",
                    Estado = 0
                },
                new Libro {
                    Titulo = "La sombra del viento",
                    Autor = "Carlos Ruiz Zafón",
                    Editorial = "Planeta",
                    ISBN = "978-8408043643",
                    Subcategoria = "Misterio gótico",
                    TipoMaterial = "Novela",
                    Estado = 0
                },
                new Libro {
                    Titulo = "Breves respuestas a grandes preguntas",
                    Autor = "Stephen Hawking",
                    Editorial = "Crítica",
                    ISBN = "978-8491990440",
                    Subcategoria = "Divulgación científica",
                    TipoMaterial = "Ensayo",
                    Estado = 0
                },
                new Libro {
                    Titulo = "Harry Potter y la piedra filosofal",
                    Autor = "J.K. Rowling",
                    Editorial = "Salamandra",
                    ISBN = "978-8498381405",
                    Subcategoria = "Fantasía juvenil",
                    TipoMaterial = "Novela",
                    Estado = 0
                },
                new Libro {
                    Titulo = "Crónica de una muerte anunciada",
                    Autor = "Gabriel García Márquez",
                    Editorial = "Penguin Random House",
                    ISBN = "978-0307474729",
                    Subcategoria = "Realismo mágico",
                    TipoMaterial = "Novela",
                    Estado = 0
                },
                new Libro {
                    Titulo = "Rebelión en la granja",
                    Autor = "George Orwell",
                    Editorial = "Secker & Warburg",
                    ISBN = "978-0451524936",
                    Subcategoria = "Sátira política",
                    TipoMaterial = "Novela",
                    Estado = 0
                },
                new Libro {
                    Titulo = "El temor de un hombre sabio",
                    Autor = "Patrick Rothfuss",
                    Editorial = "DeBolsillo",
                    ISBN = "978-8401352837",
                    Subcategoria = "Fantasía épica",
                    TipoMaterial = "Novela",
                    Estado = 0
                },
                new Libro {
                    Titulo = "Homo Deus",
                    Autor = "Yuval Noah Harari",
                    Editorial = "Debate",
                    ISBN = "978-8499926224",
                    Subcategoria = "Futuro de la humanidad",
                    TipoMaterial = "Ensayo",
                    Estado = 0
                },
                new Libro {
                    Titulo = "Emma",
                    Autor = "Jane Austen",
                    Editorial = "Alianza Editorial",
                    ISBN = "978-8420664551",
                    Subcategoria = "Romance clásico",
                    TipoMaterial = "Novela",
                    Estado = 0
                },
                new Libro {
                    Titulo = "El arte de la guerra",
                    Autor = "Sun Tzu",
                    Editorial = "Penguin Clásicos",
                    ISBN = "978-8491052011",
                    Subcategoria = "Estrategia militar",
                    TipoMaterial = "Tratado",
                    Estado = 0
                },
                new Libro {
                    Titulo = "Los pilares de la Tierra",
                    Autor = "Ken Follett",
                    Editorial = "Plaza & Janés",
                    ISBN = "978-8401337209",
                    Subcategoria = "Novela histórica",
                    TipoMaterial = "Novela",
                    Estado = 0
                },
                new Libro {
                    Titulo = "El alquimista",
                    Autor = "Paulo Coelho",
                    Editorial = "Planeta",
                    ISBN = "978-8408146765",
                    Subcategoria = "Ficción espiritual",
                    TipoMaterial = "Novela",
                    Estado = 0
                },
                new Libro {
                    Titulo = "El señor de los anillos",
                    Autor = "J.R.R. Tolkien",
                    Editorial = "Minotauro",
                    ISBN = "978-8445000667",
                    Subcategoria = "Fantasía heroica",
                    TipoMaterial = "Novela",
                    Estado = 0
                },
                new Libro {
                    Titulo = "El retrato de Dorian Gray",
                    Autor = "Oscar Wilde",
                    Editorial = "Alianza Editorial",
                    ISBN = "978-8420664552",
                    Subcategoria = "Clásico gótico",
                    TipoMaterial = "Novela",
                    Estado = 0
                }
            };

                context.Libros.AddRange(libros);
                context.SaveChanges();
                Console.WriteLine("Libros creados exitosamente");
            }
        }
    }
}
