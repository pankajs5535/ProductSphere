# ===========================
# Build Stage
# ===========================

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore "../ProductSphere.sln"

RUN dotnet publish "ProductSphere.API/ProductSphere.API.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


# ===========================
# Runtime Stage
# ===========================

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "ProductSphere.API.dll"]