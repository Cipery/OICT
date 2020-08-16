FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["OICT.Api/OICT.Api.csproj", "OICT.Api/"]
COPY ["OICT.Application/OICT.Application.csproj", "OICT.Application/"]
COPY ["OICT.Infrastructure/OICT.Infrastructure.csproj", "OICT.Infrastructure/"]
COPY ["OICT.Domain/OICT.Domain.csproj", "OICT.Domain/"]
RUN dotnet restore "OICT.Api/OICT.Api.csproj"
COPY . .
WORKDIR "/src/OICT.Api"
RUN dotnet build "OICT.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OICT.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OICT.Api.dll"]
