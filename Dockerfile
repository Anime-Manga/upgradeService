#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Cesxhin.AnimeManga.UpgradeService/", "./Cesxhin.AnimeManga.UpgradeService/"]
COPY ["src/refereces/Cesxhin.AnimeManga.Domain/", "./refereces/Cesxhin.AnimeManga.Domain/"]
COPY ["src/refereces/Cesxhin.AnimeManga.Application/", "./refereces/Cesxhin.AnimeManga.Application/"]

RUN dotnet restore "./Cesxhin.AnimeManga.UpgradeService/Cesxhin.AnimeManga.UpgradeService.csproj"

COPY . .
WORKDIR "./Cesxhin.AnimeManga.UpgradeService"

RUN dotnet build "Cesxhin.AnimeManga.UpgradeService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cesxhin.AnimeManga.UpgradeService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cesxhin.AnimeManga.UpgradeService.dll"]