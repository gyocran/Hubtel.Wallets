FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY /Hubtel.Wallets.Api/*.csproj .
RUN dotnet restore "Hubtel.Wallets.Api.csproj"

# copy everything else and build app
COPY . .
WORKDIR /app/Hubtel.Wallets.Api
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/Hubtel.Wallets.Api/out .
ENTRYPOINT ["dotnet", "Hubtel.Wallets.Api.dll"]