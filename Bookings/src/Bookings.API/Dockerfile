FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Bookings/src/Bookings.API/Bookings.API.csproj", "Bookings/src/Bookings.API/"]
COPY ["SharedLibs/Shared.AspNet/Shared.AspNet.csproj", "SharedLibs/Shared.AspNet/"]
COPY ["SharedLibs/Shared.Failures/Shared.Failures.csproj", "SharedLibs/Shared.Failures/"]
COPY ["SharedLibs/Shared.Objects/Shared.Objects.csproj", "SharedLibs/Shared.Objects/"]
COPY ["SharedLibs/Shared.Enums/Shared.Enums.csproj", "SharedLibs/Shared.Enums/"]
COPY ["SharedLibs/Shared.Infrastructure.Security/Shared.Infrastructure.Security.csproj", "SharedLibs/Shared.Infrastructure.Security/"]
COPY ["Bookings/src/Bookings.Application/Bookings.Application.csproj", "Bookings/src/Bookings.Application/"]
COPY ["SharedLibs/Shared.Messaging/Shared.Messaging.csproj", "SharedLibs/Shared.Messaging/"]
COPY ["Bookings/src/Bookings.Domain/Bookings.Domain.csproj", "Bookings/src/Bookings.Domain/"]
COPY ["Bookings/src/Bookings.Infrastructure/Bookings.Infrastructure.csproj", "Bookings/src/Bookings.Infrastructure/"]
COPY ["SharedLibs/Shared.Infrastructure.Kafka/Shared.Infrastructure.Kafka.csproj", "SharedLibs/Shared.Infrastructure.Kafka/"]
RUN dotnet restore "Bookings/src/Bookings.API/Bookings.API.csproj"
COPY . .
WORKDIR "/src/Bookings/src/Bookings.API"
RUN dotnet build "./Bookings.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Bookings.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bookings.API.dll"]
