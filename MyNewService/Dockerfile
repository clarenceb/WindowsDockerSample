# Build layer
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8.1 AS build
WORKDIR /app

# copy everything and build app
COPY . .
RUN msbuild MyNewService.csproj -t:restore 
RUN msbuild MyNewService.csproj -p:Configuration=Release

# Runtime layer
FROM mcr.microsoft.com/dotnet/framework/runtime:4.8.1
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

WORKDIR /app
COPY --from=build /app/bin/Release/ ./

#install service using powershell or installutil.exe 
RUN "C:/Windows/Microsoft.NET/Framework64/v4.0.30319/installutil.exe" \
  /LogToConsole=true /ShowCallStack \
  C:/app/MyNewService.exe ;

RUN mkdir C:\uploads

#The container stays up as long as this process is running.
ENTRYPOINT ["powershell"] 
CMD Start-Service -Name "MyNewService"; \
    Get-EventLog -LogName DemoServiceLog -Source DemoLogSource -After (Get-Date).AddHours(-1) | Format-List ;\
    $idx = (get-eventlog -LogName DemoServiceLog -Source DemoLogSource -Newest 1).Index; \
    while ($true) \
    {; \
      start-sleep -Seconds 1; \
      $idx2  = (Get-EventLog -LogName DemoServiceLog -Source DemoLogSource -newest 1).index; \
      get-eventlog -logname DemoServiceLog -Source DemoLogSource -newest ($idx2 - $idx) |  sort index | Format-List; \
      $idx = $idx2; \
    }
