# Etapa 1: construir la app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Carpeta base
WORKDIR /src

# Copiar todo el repo
COPY . .

# 👇 MUY IMPORTANTE:
# Cambia "AplicacionTacho" si tu carpeta de proyecto tiene otro nombre.
WORKDIR /src/AplicacionTacho

# Restaurar paquetes
RUN dotnet restore

# Publicar en modo Release a la carpeta /app
RUN dotnet publish -c Release -o /app

# Etapa 2: runtime ligero
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar lo publicado
COPY --from=build /app .

# Puerto para Render
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Nombre de tu DLL
ENTRYPOINT ["dotnet", "AplicacionTacho.dll"]
