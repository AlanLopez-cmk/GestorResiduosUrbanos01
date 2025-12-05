# Etapa 1: construir la app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todo el código al contenedor
COPY . .

# Restaurar paquetes
RUN dotnet restore

# Publicar en modo Release a la carpeta /app
RUN dotnet publish -c Release -o /app

# Etapa 2: imagen ligera para ejecutar
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar lo publicado desde la etapa de build
COPY --from=build /app .

# Render detecta el puerto expuesto
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# 👇 Aquí va el nombre de tu DLL
ENTRYPOINT ["dotnet", "AplicacionTacho.dll"]

