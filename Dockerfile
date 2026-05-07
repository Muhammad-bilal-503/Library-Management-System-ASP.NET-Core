# =============================================
# BookVault Library Management System
# Dockerfile — Multi-stage build
# =============================================

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY LibraryMS.csproj .
RUN dotnet restore

# Copy all source files
COPY . .

# Build and publish in Release mode
RUN dotnet publish -c Release -o /app/publish

# =============================================
# Stage 2: Runtime (smaller final image)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish .

# Expose port 8080
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application
ENTRYPOINT ["dotnet", "LibraryMS.dll"]
