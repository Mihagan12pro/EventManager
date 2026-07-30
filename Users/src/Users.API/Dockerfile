FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Users/src/Users.API/Users.API.csproj", "Users/src/Users.API/"]
COPY ["SharedLibs/Shared.AspNet/Shared.AspNet.csproj", "SharedLibs/Shared.AspNet/"]
COPY ["SharedLibs/Shared.Failures/Shared.Failures.csproj", "SharedLibs/Shared.Failures/"]
COPY ["SharedLibs/Shared.Objects/Shared.Objects.csproj", "SharedLibs/Shared.Objects/"]
COPY ["Users/src/Users.Application/Users.Application.csproj", "Users/src/Users.Application/"]
COPY ["SharedLibs/Shared.Enums/Shared.Enums.csproj", "SharedLibs/Shared.Enums/"]
COPY ["Users/src/Users.Domain/Users.Domain.csproj", "Users/src/Users.Domain/"]
COPY ["SharedLibs/Shared.Validation/Shared.Validation.csproj", "SharedLibs/Shared.Validation/"]
COPY ["Users/src/Users.Infrastructure.Postgre/Users.Infrastructure.Postgre.csproj", "Users/src/Users.Infrastructure.Postgre/"]
COPY ["Users/src/Users.Infrastructure.Security/Users.Infrastructure.Security.csproj", "Users/src/Users.Infrastructure.Security/"]
RUN dotnet restore "Users/src/Users.API/Users.API.csproj"
COPY . .
WORKDIR "/src/Users/src/Users.API"
RUN dotnet build "./Users.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Users.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Users.API.dll"]
