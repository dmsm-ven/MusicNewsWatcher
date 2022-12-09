#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MusicNewsWatcher.ConsoleClient/MusicNewsWatcher.BotService.csproj", "MusicNewsWatcher.ConsoleClient/"]
COPY ["MusicNewsWatcher.TelegramBot/MusicNewsWatcher.TelegramBot.csproj", "MusicNewsWatcher.TelegramBot/"]
COPY ["MusicNewsWatcher.Core/MusicNewsWatcher.Core.csproj", "MusicNewsWatcher.Core/"]
RUN dotnet restore "MusicNewsWatcher.ConsoleClient/MusicNewsWatcher.BotService.csproj"
COPY . .
WORKDIR "/src/MusicNewsWatcher.ConsoleClient"
RUN dotnet build "MusicNewsWatcher.BotService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MusicNewsWatcher.BotService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MusicNewsWatcher.BotService.dll"]