# Take a base image from the public Docker Hub repositories
FROM  mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
# Navigate to the “/app” folder (create if not exists)
WORKDIR /app
# Copy csproj and download the dependencies listed in that file
COPY *.csproj ./
RUN dotnet restore
# Copy all files in the project folder
COPY . ./
RUN dotnet publish -c Release -o out
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "awesomeMVC.dll"]
~                                           
