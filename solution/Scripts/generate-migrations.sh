#!/usr/bin/env bash

pushd ..

rm -rf Projects/Signaller.Apps.DbApp/Data/Migrations

dotnet ef migrations add CreateSchema --project Projects/Signaller.Apps.DbApp/Signaller.Apps.DbApp.csproj --context Signaller.Data.ApplicationDbContext --output-dir Data/Migrations/ApplicationDb
dotnet ef migrations add CreateSchema --project Projects/Signaller.Apps.DbApp/Signaller.Apps.DbApp.csproj --context Signaller.Data.KeyDbContext --output-dir Data/Migrations/KeyDb
dotnet ef migrations add CreateSchema --project Projects/Signaller.Apps.DbApp/Signaller.Apps.DbApp.csproj --context IdentityServer4.EntityFramework.DbContexts.PersistedGrantDbContext --output-dir Data/Migrations/PersistedGrantDb
dotnet ef migrations add CreateSchema --project Projects/Signaller.Apps.DbApp/Signaller.Apps.DbApp.csproj --context IdentityServer4.EntityFramework.DbContexts.ConfigurationDbContext --output-dir Data/Migrations/ConfigurationDb

dotnet restore
dotnet build

popd