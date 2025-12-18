# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["src/LibrarySystem.Web/LibrarySystem.Web.csproj", "src/LibrarySystem.Web/"]
COPY ["src/LibrarySystem.Application/LibrarySystem.Application.csproj", "src/LibrarySystem.Application/"]
COPY ["src/LibrarySystem.Domain/LibrarySystem.Domain.csproj", "src/LibrarySystem.Domain/"]
COPY ["src/LibrarySystem.Infrastructure/LibrarySystem.Infrastructure.csproj", "src/LibrarySystem.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/LibrarySystem.Web/LibrarySystem.Web.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/LibrarySystem.Web"
RUN dotnet build "LibrarySystem.Web.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "LibrarySystem.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:PublishReadyToRun=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LibrarySystem.Web.dll"]

