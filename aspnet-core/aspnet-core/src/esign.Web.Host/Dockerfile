#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/esign.Web.Host/esign.Web.Host.csproj", "src/esign.Web.Host/"]
COPY ["src/esign.Web.Core/esign.Web.Core.csproj", "src/esign.Web.Core/"]
COPY ["src/esign.Application/esign.Application.csproj", "src/esign.Application/"]
COPY ["src/esign.Application.Shared/esign.Application.Shared.csproj", "src/esign.Application.Shared/"]
COPY ["src/esign.Core.Shared/esign.Core.Shared.csproj", "src/esign.Core.Shared/"]
COPY ["src/esign.Core/esign.Core.csproj", "src/esign.Core/"]
COPY ["src/esign.EntityFrameworkCore/esign.EntityFrameworkCore.csproj", "src/esign.EntityFrameworkCore/"]
COPY ["src/esign.GraphQL/esign.GraphQL.csproj", "src/esign.GraphQL/"]
RUN dotnet restore "src/esign.Web.Host/esign.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/esign.Web.Host"
RUN dotnet build "esign.Web.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "esign.Web.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "esign.Web.Host.dll"]
