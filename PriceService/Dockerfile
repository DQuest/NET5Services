FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /src

COPY ./PriceService ./PriceService
COPY ./BaseRepository ./BaseRepository
COPY ./AuthBase ./AuthBase

WORKDIR ./PriceService
RUN dotnet restore "PriceService.csproj"
RUN dotnet build "PriceService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PriceService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PriceService.dll"]