FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG DOCKER_DOTNET_PROJECT_PREFIX="BookHub.API"
ARG DOCKER_DOTNET_MAIN_PROJECT="${DOCKER_DOTNET_PROJECT_PREFIX}.API"


WORKDIR /src
COPY ["API/API.csproj", "API/"]
COPY ["Storage/Storage.csproj", "Storage/"]
RUN dotnet restore "./API/API.csproj"
COPY . .
WORKDIR "/src/API"

# RUN dotnet dev-certs https
RUN dotnet build "./API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "BookHub.API.Service.dll"]
