# Stage 1: Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Stage 2: Build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files for restore
COPY ["skillsphere-backend/skillsphere.backend.csproj", "skillsphere-backend/"]
COPY ["Infrastructure/skillsphere.infrastructure.csproj", "Infrastructure/"]
COPY ["Core/skillsphere.core.csproj", "Core/"]

RUN dotnet restore "skillsphere-backend/skillsphere.backend.csproj"

# Copy all files and build
COPY . .
WORKDIR "/src/skillsphere-backend"
RUN dotnet publish "skillsphere.backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Tell .NET to listen on port 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "skillsphere.backend.dll"]
