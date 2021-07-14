FROM mcr.microsoft.com/dotnet/sdk:5.0

WORKDIR /app

COPY *.sln .
COPY Projects/Signaller/*.csproj Projects/Signaller/
COPY Projects/Signaller.Apps.ApiApp/*.csproj Projects/Signaller.Apps.ApiApp/
COPY Projects/Signaller.Apps.WebApp/*.csproj Projects/Signaller.Apps.WebApp/
COPY Projects/Signaller.Apps.DbApp/*.csproj Projects/Signaller.Apps.DbApp/

RUN dotnet restore

COPY Projects/Signaller/ Projects/Signaller/
COPY Projects/Signaller.Apps.ApiApp/ Projects/Signaller.Apps.ApiApp/
COPY Projects/Signaller.Apps.WebApp/ Projects/Signaller.Apps.WebApp/
COPY Projects/Signaller.Apps.DbApp/ Projects/Signaller.Apps.DbApp/

COPY Keys/ Keys/
