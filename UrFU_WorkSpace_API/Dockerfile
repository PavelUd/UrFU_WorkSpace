FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /api

COPY *.csproj ./
RUN dotnet restore UrFU_WorkSpace_API.csproj

COPY . ./
RUN dotnet publish -c Release -o output

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /api
COPY --from=builder /api/output ./
ENTRYPOINT ["dotnet", "UrFU_WorkSpace_API.dll"]
