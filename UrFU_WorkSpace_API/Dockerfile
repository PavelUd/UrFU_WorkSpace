FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /api

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UrFU_WorkSpace_API.csproj", "UrFU_WorkSpace_API.csproj"]
RUN dotnet restore "UrFU_WorkSpace_API.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "UrFU_WorkSpace_API.csproj" -c Release -o /api

FROM build AS publish
RUN dotnet publish "UrFU_WorkSpace_API.csproj" -c Release -o /api

FROM base AS final
WORKDIR /api
COPY --from=publish /api .
ENTRYPOINT ["dotnet", "UrFU_WorkSpace_API.dll"]
