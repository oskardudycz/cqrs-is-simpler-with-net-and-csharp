########################################
#  First stage of multistage build
########################################
#  Use Build image with label `builder
########################################
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS builder

# Setup working directory for project
WORKDIR /app

COPY ./.editorconfig ./
COPY ./Directory.Build.props ./
COPY ./Core.Build.props ./
COPY ./Final/Warehouse/Warehouse.csproj ./Final/Warehouse/
COPY ./Final/Warehouse.Api/Warehouse.Api.csproj ./Final/Warehouse.Api/

# Restore nuget packages
RUN dotnet restore ./Final/Warehouse.Api/Warehouse.Api.csproj

# Copy project files
COPY ./Final/Warehouse ./Final/Warehouse
COPY ./Final/Warehouse.Api/ ./Final/Warehouse.Api

# Build project with Release configuration
# and no restore, as we did it already
RUN dotnet build -c Release --no-restore ./Final/Warehouse.Api/Warehouse.Api.csproj

## Test project with Release configuration
## and no build, as we did it already
#RUN dotnet test -c Release --no-build ./Final/Warehouse.Api/Warehouse.Api.Tests.csproj


# Publish project to output folder
# and no build, as we did it already
WORKDIR /app/Final/Warehouse.Api
RUN ls
RUN dotnet publish -c Release --no-build -o out

########################################
#  Second stage of multistage build
########################################
#  Use other build image as the final one
#    that won't have source codes
########################################
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine

# Setup working directory for project
WORKDIR /app

# Copy published in previous stage binaries
# from the `builder` image
COPY --from=builder /app/Final/Warehouse.Api/out .

# Set URL that App will be exposed
ENV ASPNETCORE_URLS="http://*:5000"

# sets entry point command to automatically
# run application on `docker run`
ENTRYPOINT ["dotnet", "Warehouse.Api.dll"]
