# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /src
EXPOSE 80
EXPOSE 443

# Use the .NET SDK image to build the project files
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["API/API.csproj", "./API/"]
COPY ["Persistence/Persistence.csproj", "./Persistence/"]
COPY ["Domain/Domain.csproj", "./Domain/"]
COPY ["Application/Application.csproj", "./Application/"]
RUN dotnet restore "API/API.csproj"
COPY . .
RUN dotnet build "API/API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API/API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
