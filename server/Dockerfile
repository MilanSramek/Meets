# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release

# Copy property files
COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]
COPY ["src/common/Directory.Build.props", "src/common/"]
COPY ["src/common/persistence/Directory.Build.props", "src/common/persistence/"]
COPY ["src/common/presentation/Directory.Build.props", "src/common/presentation/"]
COPY ["src/scheduler/Directory.Build.props", "src/scheduler/"]
COPY ["src/scheduler/persistence/Directory.Build.props", "src/scheduler/persistence/"]
COPY ["src/scheduler/presentation/Directory.Build.props", "src/scheduler/presentation/"]

# Copy all project files
COPY ["src/common/application/Application.csproj", "src/common/application/"]
COPY ["src/common/domain/Domain.csproj", "src/common/domain/"]
COPY ["src/common/expansions/Expansions.csproj", "src/common/expansions/"]
COPY ["src/common/infrastructure/Infrastructure.csproj", "src/common/infrastructure/"]
COPY ["src/common/persistence/mongodb/Persistence.MongoDb.csproj", "src/common/persistence/mongodb/"]
COPY ["src/common/presentation/graphql/Presentation.GraphQL.csproj", "src/common/presentation/graphql/"]
COPY ["src/common/tools/Tools.csproj", "src/common/tools/"]
COPY ["src/root/Meets.csproj", "src/root/"]
COPY ["src/scheduler/application/Application.csproj", "src/scheduler/application/"]
COPY ["src/scheduler/domain/Domain.csproj", "src/scheduler/domain/"]
COPY ["src/scheduler/persistence/mongodb/Persistence.MongoDb.csproj", "src/scheduler/persistence/mongodb/"]
COPY ["src/scheduler/presentation/graphql/Presentation.GraphQL.csproj", "src/scheduler/presentation/graphql/"]

RUN dotnet restore "./src/root/Meets.csproj"

COPY . .
WORKDIR "/src/root"
RUN dotnet build "./Meets.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/root"
RUN dotnet publish "./Meets.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Meets.dll"]