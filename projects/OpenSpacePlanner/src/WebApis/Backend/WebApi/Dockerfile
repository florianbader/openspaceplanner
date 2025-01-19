ARG DOTNET_VERSION=9.0

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS packages
WORKDIR /src

# Copy the dotnet-tools.json files and restore the tools
# This makes sure that the Docker cache is invalidated only when the dotnet-tools.json files change
RUN --mount=type=bind,target=/docker-context \
    cd /docker-context/; \
    find ./ \( -iname "dotnet-tools.json" \) -exec cp --parents "{}" /src \;


# Copy the project files, global props and targets, and the central package versioning file and restore the dependencies
# This makes sure that the Docker cache is invalidated only when any of these files change
RUN --mount=type=bind,target=/docker-context \
cd /docker-context/; \
find ./ -mindepth 0 \( -name "*.sln" -o -name "*.csproj" -o -iname "*.targets" -o -iname "*.props" \) -exec cp --parents "{}" /src \;

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /src

COPY --from=packages /src/ .

RUN dotnet tool restore
ENV PATH="${PATH}:/root/.dotnet/tools"

RUN dotnet restore "projects/OpenSpacePlanner/src/WebApis/Backend/WebApi/Backend.WebApi.csproj"

# Copy the remaining files and build the project
COPY . .
RUN dotnet publish "projects/OpenSpacePlanner/src/WebApis/Backend/WebApi/Backend.WebApi.csproj" --no-restore -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS base
WORKDIR /app

ARG PROJECT_NAME=Backend.WebApi

COPY --from=build /app/publish .

EXPOSE 80

ENV ASPNETCORE_URLS="http://+:80"

ENTRYPOINT ["dotnet", "RioScaffolding.OpenSpacePlanner.Backend.WebApi.dll"]
