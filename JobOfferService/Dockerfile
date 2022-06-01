FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 5006

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY JobOfferService/JobOfferService.csproj JobOfferService/
RUN dotnet restore JobOfferService/JobOfferService.csproj
COPY . .
WORKDIR /src/JobOfferService
RUN dotnet build JobOfferService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish JobOfferService.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "JobOfferService.dll"]