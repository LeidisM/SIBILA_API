# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos del proyecto
COPY . .

# Restaurar dependencias
RUN dotnet restore

# Publicar en modo Release
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copiar archivos publicados desde el build
COPY --from=build /app/publish .

# Exponer el puerto (Render usa el 80)
EXPOSE 80

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "SIBILA_API.dll"]
