FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Events/src/Events.API/Events.API.csproj", "Events/src/Events.API/"]
COPY ["SharedLibs/Shared.AspNet/Shared.AspNet.csproj", "SharedLibs/Shared.AspNet/"]
COPY ["SharedLibs/Shared.Failures/Shared.Failures.csproj", "SharedLibs/Shared.Failures/"]
COPY ["SharedLibs/Shared.Objects/Shared.Objects.csproj", "SharedLibs/Shared.Objects/"]
COPY ["SharedLibs/Shared.Infrastructure.Security/Shared.Infrastructure.Security.csproj", "SharedLibs/Shared.Infrastructure.Security/"]
COPY ["Events/src/Events.Application/Events.Application.csproj", "Events/src/Events.Application/"]
COPY ["SharedLibs/Shared.Messaging/Shared.Messaging.csproj", "SharedLibs/Shared.Messaging/"]
COPY ["Events/src/Events.Domain/Events.Domain.csproj", "Events/src/Events.Domain/"]
COPY ["SharedLibs/Shared.Validation/Shared.Validation.csproj", "SharedLibs/Shared.Validation/"]
COPY ["Events/src/Events.Infrastracture/Events.Infrastracture.csproj", "Events/src/Events.Infrastracture/"]
COPY ["SharedLibs/Shared.Infrastructure.Kafka/Shared.Infrastructure.Kafka.csproj", "SharedLibs/Shared.Infrastructure.Kafka/"]
RUN dotnet restore "Events/src/Events.API/Events.API.csproj"
COPY . .
WORKDIR "/src/Events/src/Events.API"
RUN dotnet build "./Events.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Events.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Events.API.dll"]
