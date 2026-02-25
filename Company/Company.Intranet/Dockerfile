# See https://aka.ms/customizecontainer to learn how to customize your debug container

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8084
EXPOSE 8085

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY ["Company.Intranet/Company.Intranet.csproj", "./Company.Intranet/"]
COPY ["Company.Data/Company.Data.csproj", "./Company.Data/"]
RUN dotnet restore "./Company.Intranet/Company.Intranet.csproj"

# Copy everything else and build
COPY Company.Intranet/. ./Company.Intranet/
COPY Company.Data/. ./Company.Data/
WORKDIR "/src/Company.Intranet"
RUN dotnet build "./Company.Intranet.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Company.Intranet.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Company.Intranet.dll"]

