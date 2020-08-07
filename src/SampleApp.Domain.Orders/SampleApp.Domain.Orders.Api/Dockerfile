#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Sample.Web/Sample.Web.csproj", "Sample.Web/"]
COPY ["Sample.Infrastructure/Sample.Infrastructure.csproj", "Sample.Infrastructure/"]
COPY ["Sample.Domain/Sample.Domain.csproj", "Sample.Domain/"]
RUN dotnet restore "Sample.Web/Sample.Web.csproj"
COPY . .
WORKDIR "/src/Sample.Web"
RUN dotnet build "Sample.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.Web.dll"]