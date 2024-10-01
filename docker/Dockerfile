FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BooksService/BooksService.csproj", "BooksService/"]
COPY ["Storage/Storage.csproj", "Storage/"]
RUN dotnet restore "./BooksService/BooksService.csproj"
COPY . .
WORKDIR "/src/BooksService"

RUN dotnet dev-certs https
RUN dotnet build "./BooksService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BooksService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BooksService.dll"]