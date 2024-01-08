#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Server/ChatRoom.Api/ChatRoom.Api.csproj", "src/Server/ChatRoom.Api/"]
COPY ["src/Server/ChatRoom.Application/ChatRoom.Application/ChatRoom.Application.csproj", "src/Server/ChatRoom.Application/ChatRoom.Application/"]
COPY ["src/Framework/ChatRoom.Framework/ChatRoom.Framework.csproj", "src/Framework/ChatRoom.Framework/"]
COPY ["src/Server/ChatRoom.DataLayer/ChatRoom.Infrastructure.csproj", "src/Server/ChatRoom.DataLayer/"]
RUN dotnet restore "./src/Server/ChatRoom.Api/./ChatRoom.Api.csproj"
COPY . .
WORKDIR "/src/src/Server/ChatRoom.Api"
RUN dotnet build "./ChatRoom.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ChatRoom.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatRoom.Api.dll"]