FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /src

COPY ./ProductService ./ProductService
COPY ./AuthBase ./AuthBase
COPY ./BaseRepository ./BaseRepository

WORKDIR ./ProductService
RUN dotnet restore "ProductService.csproj"
RUN dotnet build "ProductService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductService.dll"]