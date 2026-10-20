ARG ARCH=amd64
ARG VERSION=8.0
ARG PLAYWRIGHT_DOWNLOAD_CONNECTION_TIMEOUT=120000
# Ubuntu 22.04 is supported by Playwright and .NET 8.
ARG TAG=$VERSION-jammy-$ARCH

FROM mcr.microsoft.com/dotnet/sdk:$VERSION-jammy AS build
WORKDIR /app

COPY src/iBudget.csproj ./src/
COPY Shared/Shared/Shared.csproj ./Shared/Shared/
RUN dotnet restore src/iBudget.csproj

COPY src/ ./src/
COPY Shared/ ./Shared/
RUN --mount=type=secret,id=sixlabors_license,required=true \
    dotnet publish src/iBudget.csproj -c Release -o /app/out --no-restore \
    -p:SixLaborsLicenseFile=/run/secrets/sixlabors_license

FROM mcr.microsoft.com/dotnet/aspnet:$TAG
ARG PLAYWRIGHT_DOWNLOAD_CONNECTION_TIMEOUT
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=80
ENV PLAYWRIGHT_BROWSERS_PATH=/ms-playwright

# Playwright installer uses PowerShell only while building the image. Chromium and
# its Linux dependencies remain in the final image.
RUN apt-get update \
    && apt-get install -yq --no-install-recommends \
        wget apt-transport-https software-properties-common \
    && wget -q https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O /tmp/packages-microsoft-prod.deb \
    && dpkg -i /tmp/packages-microsoft-prod.deb \
    && apt-get update \
    && apt-get install -yq --no-install-recommends powershell

COPY --from=build /app/out .

RUN PLAYWRIGHT_DOWNLOAD_CONNECTION_TIMEOUT=$PLAYWRIGHT_DOWNLOAD_CONNECTION_TIMEOUT \
        pwsh ./playwright.ps1 install --with-deps --only-shell chromium \
    && apt-get purge -y powershell packages-microsoft-prod wget apt-transport-https software-properties-common \
    && apt-get autoremove -y \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/* /tmp/*

EXPOSE 80 443

ENTRYPOINT ["dotnet", "iBudget.dll"]
