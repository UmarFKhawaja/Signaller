ARG BASE_IMAGE_NAME
FROM ${BASE_IMAGE_NAME} AS build

WORKDIR /app/Projects/Signaller.Apps.DbApp

RUN dotnet build Signaller.Apps.DbApp.csproj -c Release -o /app/build

FROM build AS publish

RUN dotnet publish Signaller.Apps.DbApp.csproj -c Release -o /app/publish

COPY --from=build /app/Keys/localhost.pfx /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base

WORKDIR /app

EXPOSE 80
EXPOSE 443

FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Signaller.Apps.DbApp.dll"]
