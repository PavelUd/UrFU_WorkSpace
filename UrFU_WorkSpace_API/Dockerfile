FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UrFU_WorkSpace_API.csproj", "UrFU_WorkSpace_API.csproj"]
RUN dotnet restore "UrFU_WorkSpace_API.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "UrFU_WorkSpace_API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "UrFU_WorkSpace_API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "UrFU_WorkSpace_API.dll"]
