FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SpoofSettingsService.csproj", "."]
RUN dotnet restore "SpoofSettingsService.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "SpoofSettingsService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SpoofSettingsService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SpoofSettingsService.dll"]