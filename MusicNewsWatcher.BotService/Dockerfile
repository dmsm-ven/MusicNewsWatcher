#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MusicNewsWatcher.BotService/MusicNewsWatcher.BotService.csproj", "MusicNewsWatcher.BotService/"]
COPY ["MusicNewsWatcher.Core/MusicNewsWatcher.Core.csproj", "MusicNewsWatcher.Core/"]
COPY ["MusicNewsWatcher.TelegramBot/MusicNewsWatcher.TelegramBot.csproj", "MusicNewsWatcher.TelegramBot/"]
COPY ["MusicNewWatcher.BL/MusicNewWatcher.BL.csproj", "MusicNewWatcher.BL/"]
RUN dotnet restore "./MusicNewsWatcher.BotService/./MusicNewsWatcher.BotService.csproj"
COPY . .
WORKDIR "/src/MusicNewsWatcher.BotService"
RUN dotnet build "./MusicNewsWatcher.BotService.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MusicNewsWatcher.BotService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MusicNewsWatcher.BotService.dll"]

RUN apt-get update && apt-get install -y tzdata
ENV TZ=Europe/Moscow
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone