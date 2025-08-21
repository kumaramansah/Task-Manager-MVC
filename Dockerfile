# Step 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY *.sln .
COPY TaskManager/TaskManager.csproj ./TaskManager/
RUN dotnet restore "./TaskManager/TaskManager.csproj"

# Copy all source files and publish
COPY . .
WORKDIR /src/TaskManager
RUN dotnet publish -c Release -o /app/publish

# Step 2: Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose default ASP.NET Core port
EXPOSE 5000

# Run the application
ENTRYPOINT ["dotnet", "TaskManager.dll"]
