# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia a solução e todos os projetos.
COPY ["CatalogAPI.sln", "."]
COPY ["src/CatalogAPI.Api/CatalogAPI.Api.csproj", "src/CatalogAPI.Api/"]
COPY ["src/CatalogAPI.Application/CatalogAPI.Application.csproj", "src/CatalogAPI.Application/"]
COPY ["src/CatalogAPI.Authentication/CatalogAPI.Authentication.csproj", "src/CatalogAPI.Authentication/"]
COPY ["src/CatalogAPI.Data/CatalogAPI.Data.csproj", "src/CatalogAPI.Data/"]
COPY ["src/CatalogAPI.Domain/CatalogAPI.Domain.csproj", "src/CatalogAPI.Domain/"]
COPY ["src/CatalogAPI.IoC/CatalogAPI.IoC.csproj", "src/CatalogAPI.IoC/"]
COPY ["src/CatalogAPI.Messaging/CatalogAPI.Messaging.csproj", "src/CatalogAPI.Messaging/"]

# restaura as dependencias
RUN dotnet restore "CatalogAPI.sln"

#copia o restante do codigo.
COPY . .


#publica o projeto principal
RUN dotnet publish "src/CatalogAPI.Api/CatalogAPI.Api.csproj" -c Release -o /app/publish


#img final.
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CatalogAPI.Api.dll"]