FROM mcr.microsoft.com/dotnet/core/aspnet:2.1
ADD NetJsonRpc/published /app
WORKDIR /app
ENTRYPOINT ["dotnet", "NetJsonRpc.dll"]
