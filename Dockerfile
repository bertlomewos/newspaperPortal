# Use the official .NET 9.0 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY newspaperPortal/*.csproj ./newspaperPortal/
RUN dotnet restore newspaperPortal/newspaperPortal.csproj

# Copy the rest of the code and publish
COPY newspaperPortal/. ./newspaperPortal/
RUN dotnet publish newspaperPortal/newspaperPortal.csproj -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "newspaperPortal.dll"]
