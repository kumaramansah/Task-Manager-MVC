# Step 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY *.sln .
COPY TaskManager.csproj .
RUN dotnet restore

# Copy all source files
COPY . .

# Publish from the current directory (no need to change WORKDIR)
RUN dotnet publish -c Release -o /app/publish

# Step 2: Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose default ASP.NET Core port
EXPOSE 5000
EXPOSE 5001

# Run the application
ENTRYPOINT ["dotnet", "TaskManager.dll"]