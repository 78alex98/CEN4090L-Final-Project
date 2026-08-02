FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

COPY Bartering.Api.sln ./
COPY ./**/Bartering.Data.csproj ./Bartering.Data/
COPY ./**/Bartering.Tests.Integration.csproj ./Bartering.Tests.Integration/
COPY ./**/Bartering.Core.csproj ./Bartering.Core/
RUN dotnet restore --no-cache -v m

COPY . ./
RUN dotnet build ./Bartering.Core/ --no-restore -o out -v m

FROM mcr.microsoft.com/dotnet/aspnet:9.0-noble-chiseled-extra AS base
EXPOSE 8080
WORKDIR /api
COPY --from=build /src/out .
ENTRYPOINT ["dotnet", "Bartering.Core.dll"]
