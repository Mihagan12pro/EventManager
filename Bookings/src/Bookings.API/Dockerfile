FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /BookingsApp

COPY publish/ ./

ENTRYPOINT [ "dotnet", "Bookings.API.dll" ]

