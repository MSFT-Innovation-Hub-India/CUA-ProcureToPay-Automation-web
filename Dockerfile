# Use the official ASP.NET Core runtime image as a base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ERPWeb.csproj", "./"]
RUN dotnet restore "./ERPWeb.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ERPWeb.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "ERPWeb.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ERPWeb.dll"]
