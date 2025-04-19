using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SIBILA_API.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = false, // Puedes cambiar a true si quieres validar el issuer
        ValidateAudience = false, // Puedes cambiar a true si quieres validar el audience
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Habilitar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://sibilaappfront.onrender.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var dbProvider = builder.Configuration["DatabaseProvider"];

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (dbProvider == "SqlServer")
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
    }
    else
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
    }
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation($"Verificando estado de {dbProvider}...");

        // Comprobar si se puede conectar a la base de datos
        if (!context.Database.CanConnect())
        {
            logger.LogInformation($"La base de datos {dbProvider} no existe, aplicando migraciones...");
            context.Database.Migrate();
            logger.LogInformation("Base de datos creada y migraciones aplicadas");

            // Insertar datos iniciales
            SeedData.Initialize(services);
        }
        else
        {
            // Verificar si hay migraciones pendientes
            //var pendingMigrations = context.Database.GetPendingMigrations().ToList();
            //if (pendingMigrations.Any())
            //{
            //    logger.LogInformation($"Aplicando {pendingMigrations.Count} migraciones pendientes para {dbProvider}...");
            //    context.Database.Migrate();
            //    logger.LogInformation("Migraciones pendientes aplicadas");

            //    // Opcional: Verificar si necesita datos iniciales
            //    if (!context.Usuarios.Any() || !context.Libros.Any())
            //    {
            //        SeedData.Initialize(services);
            //    }
            //}
        }

        // Verificación final de datos
        if (!context.Usuarios.Any())
        {
            logger.LogWarning("No se encontraron usuarios, insertando datos iniciales...");
            SeedData.Initialize(services);
        }
    }
    catch (Exception ex)
    {        
        throw;        
    }
}

// Habilitar CORS
app.UseCors("PermitirFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
