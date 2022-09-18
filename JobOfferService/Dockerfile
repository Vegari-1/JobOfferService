FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
ARG github_username
ARG github_pat
WORKDIR /src
COPY JobOfferService/JobOfferService.csproj JobOfferService/
COPY config/NuGet.Config .
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