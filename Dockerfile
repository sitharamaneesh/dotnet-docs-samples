# First stage: Build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy the .csproj and restore any dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the files and build the project
COPY . ./
RUN dotnet publish -c Release -o out

# Second stage: Setup the runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Copy files from the build stage
COPY --from=build-env /app/out .

# Set the environment
ENV ASPNETCORE_URLS=http://*:5000

# Expose the port
EXPOSE 5000

# Run the application
ENTRYPOINT ["dotnet", "HelloWorldApp.dll"]
