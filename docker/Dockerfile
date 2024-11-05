FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG DOCKER_DOTNET_PROJECT_PREFIX="BookHub"
ARG DOCKER_DOTNET_MAIN_PROJECT="${DOCKER_DOTNET_PROJECT_PREFIX}.API"


WORKDIR /src
COPY ["${DOCKER_DOTNET_MAIN_PROJECT}/${DOCKER_DOTNET_MAIN_PROJECT}.csproj", "${DOCKER_DOTNET_MAIN_PROJECT}/"]
COPY ["${DOCKER_DOTNET_PROJECT_PREFIX}.Storage/${DOCKER_DOTNET_PROJECT_PREFIX}.Storage.csproj", "${DOCKER_DOTNET_PROJECT_PREFIX}.Storage/"]
RUN dotnet restore "./${DOCKER_DOTNET_MAIN_PROJECT}/${DOCKER_DOTNET_MAIN_PROJECT}.csproj"
COPY . .
WORKDIR "/src/${DOCKER_DOTNET_MAIN_PROJECT}"

# RUN dotnet dev-certs https
RUN dotnet build "./${DOCKER_DOTNET_MAIN_PROJECT}.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./${DOCKER_DOTNET_MAIN_PROJECT}.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_TestConfig_Content="Docker content"
ENTRYPOINT ["dotnet", "BookHub.API.dll"]
