# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY *.sln .
COPY Task-Manager-MVC/*.csproj ./Task-Manager-MVC/
RUN dotnet restore

# Copy everything and build
COPY . .
WORKDIR /src/Task-Manager-MVC
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Task-Manager-MVC.dll"]
