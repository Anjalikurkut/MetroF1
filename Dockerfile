# Stage 1: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies (leverages Docker caching)
COPY metro/Metro_Ticket_Booking/Metro_Ticket_Booking/*.csproj ./Metro_Ticket_Booking/
RUN dotnet restore Metro_Ticket_Booking/Metro_Ticket_Booking.csproj

# Copy the rest of the application source code
COPY metro/Metro_Ticket_Booking/Metro_Ticket_Booking/ ./Metro_Ticket_Booking/
WORKDIR /src/Metro_Ticket_Booking

# Build and publish the project
RUN dotnet publish Metro_Ticket_Booking.csproj -c Release -o /app/publish

# Stage 2: Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish ./

# (Optional) Use non-root user for security
# RUN useradd -m -r -u 1000 appuser
# USER appuser

# Expose application port
EXPOSE 80

# Entry point (starts your .NET app)
ENTRYPOINT ["dotnet", "Metro_Ticket_Booking.dll"]
