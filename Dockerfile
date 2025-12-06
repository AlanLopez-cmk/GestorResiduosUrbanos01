FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todo el repo
COPY . .

# 👇 MUY IMPORTANTE: entra a la carpeta del proyecto
WORKDIR /src/AplicacionTacho

# Restaurar paquetes
RUN dotnet restore

# Publicar en Release a /app
RUN dotnet publish -c Release -o /app

# Imagen final de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "AplicacionTacho.dll"]
