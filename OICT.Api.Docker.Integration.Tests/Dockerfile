#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["OICT.Api.Docker.Integration.Tests/OICT.Api.Docker.Integration.Tests.csproj", "OICT.Api.Docker.Integration.Tests/"]
COPY ["OICT.Api/OICT.Api.csproj", "OICT.Api/"]
COPY ["OICT.Application/OICT.Application.csproj", "OICT.Application/"]
COPY ["OICT.Infrastructure/OICT.Infrastructure.csproj", "OICT.Infrastructure/"]
COPY ["OICT.Domain/OICT.Domain.csproj", "OICT.Domain/"]
COPY ["OICT.Tests.Common/OICT.Tests.Common.csproj", "OICT.Tests.Common/"]
RUN dotnet restore "OICT.Api.Docker.Integration.Tests/OICT.Api.Docker.Integration.Tests.csproj"
COPY . .
WORKDIR "/src/OICT.Api.Docker.Integration.Tests"
