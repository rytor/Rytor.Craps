FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

ENV DB_HOST=db_rytorcraps
ENV DB_PORT=5432
ENV DB_USER=placeholder
ENV DB_PASSWORD=placeholder

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 5000
ENV ASPNETCORE_URlS=http://*:5000
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Rytor.Craps.Microservices.Balance.dll"]
