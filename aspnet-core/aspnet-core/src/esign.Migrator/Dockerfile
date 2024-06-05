#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/esign.Migrator/esign.Migrator.csproj", "src/esign.Migrator/"]
COPY ["src/esign.EntityFrameworkCore/esign.EntityFrameworkCore.csproj", "src/esign.EntityFrameworkCore/"]
COPY ["src/esign.Core/esign.Core.csproj", "src/esign.Core/"]
COPY ["src/esign.Core.Shared/esign.Core.Shared.csproj", "src/esign.Core.Shared/"]
RUN dotnet restore "src/esign.Migrator/esign.Migrator.csproj"
COPY . .
WORKDIR "/src/src/esign.Migrator"
RUN dotnet build "esign.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "esign.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "esign.Migrator.dll", "-s"]
