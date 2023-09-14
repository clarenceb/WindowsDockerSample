# escape=\

# Stage 1 - Build
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8.1-windowsservercore-ltsc2022 AS build
# Windows 10 or Windows Server 2019
# FROM mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019 AS build
# Windows 11 or Windows Server 2022
# FROM mcr.microsoft.com/dotnet/framework/sdk:4.8.1-windowsservercore-ltsc2022 AS build

WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./
COPY *.config ./
RUN nuget restore -PackagesDirectory ./packages

# copy everything else and build app
WORKDIR /app/WebUploaderApp
COPY . ./
RUN msbuild WebUploaderApp.csproj -t:restore
RUN msbuild WebUploaderApp.csproj -p:Configuration=Release

# Stage 2 - Runtime
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8.1-windowsservercore-ltsc2022 AS runtime
# Windows 10 or Windows Server 2019
# FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019 AS build
# Windows 11 or Windows Server 2022
# FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8.1-windowsservercore-ltsc2022 AS build

WORKDIR /inetpub/wwwroot
RUN mkdir c:\uploads
COPY --from=build /app/WebUploaderApp/. ./
