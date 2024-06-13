#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AnNguyen.Spa/annguyen.spa.client/nuget.config", "AnNguyen.Spa/annguyen.spa.client/"]
COPY ["AnNguyen.Spa/AnNguyen.Spa.Server/AnNguyen.Spa.Server.csproj", "AnNguyen.Spa/AnNguyen.Spa.Server/"]
COPY ["AnNguyen.Abtraction/AnNguyen.Abtraction.csproj", "AnNguyen.Abtraction/"]
COPY ["AnNguyen.Services/AnNguyen.Services.csproj", "AnNguyen.Services/"]
COPY ["AnNguyen.Spa/annguyen.spa.client/annguyen.spa.client.esproj", "AnNguyen.Spa/annguyen.spa.client/"]
RUN dotnet restore "./AnNguyen.Spa/AnNguyen.Spa.Server/AnNguyen.Spa.Server.csproj"
COPY . .

RUN apt-get update -yq &&  \
  apt-get upgrade -yq &&   \
  apt-get install -yq curl

RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash - && apt-get install -y nodejs

WORKDIR "/src/AnNguyen.Spa/annguyen.spa.client"
RUN npm i

WORKDIR "/src/AnNguyen.Spa/AnNguyen.Spa.Server"
RUN dotnet build "./AnNguyen.Spa.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AnNguyen.Spa.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AnNguyen.Spa.Server.dll"]