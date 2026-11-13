ARG VERSION=8.0
ARG PLAYWRIGHT_DOWNLOAD_CONNECTION_TIMEOUT=120000

FROM mcr.microsoft.com/dotnet/sdk:${VERSION}-noble AS build
WORKDIR /app
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_NOLOGO=1

COPY src/iBudget.csproj ./src/
COPY Shared/Shared/Shared.csproj ./Shared/Shared/
COPY test/iBudget.UnitTest/iBudget.UnitTest.csproj ./test/iBudget.UnitTest/
RUN dotnet restore test/iBudget.UnitTest/iBudget.UnitTest.csproj

COPY src/ ./src/
COPY Shared/ ./Shared/
COPY test/iBudget.UnitTest/ ./test/iBudget.UnitTest/

RUN --mount=type=secret,id=SIXLABORS_LICENSE_KEY,required=true \
    dotnet test test/iBudget.UnitTest/iBudget.UnitTest.csproj -c Release --no-restore \
      --logger "console;verbosity=normal" \
      -p:SixLaborsLicenseFile=/run/secrets/SIXLABORS_LICENSE_KEY \
    && dotnet publish src/iBudget.csproj -c Release -o /app/out --no-restore \
      -p:SixLaborsLicenseFile=/run/secrets/SIXLABORS_LICENSE_KEY

FROM mcr.microsoft.com/dotnet/aspnet:${VERSION}-noble AS runtime
ARG TARGETARCH
ARG PLAYWRIGHT_DOWNLOAD_CONNECTION_TIMEOUT
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=80 \
    PLAYWRIGHT_BROWSERS_PATH=/ms-playwright

COPY --from=build /app/out .
# Use the Node binary bundled with the exact Microsoft.Playwright NuGet package.
# SDK, test source and BuildKit secrets do not enter the runtime stage.
RUN case "$TARGETARCH" in \
        arm64) node_arch=arm64 ;; \
        amd64) node_arch=x64 ;; \
        *) printf 'Unsupported Playwright architecture: %s\n' "$TARGETARCH" >&2; exit 1 ;; \
    esac \
    && PLAYWRIGHT_DOWNLOAD_CONNECTION_TIMEOUT=$PLAYWRIGHT_DOWNLOAD_CONNECTION_TIMEOUT \
       ./.playwright/node/linux-$node_arch/node ./.playwright/package/cli.js install --with-deps --only-shell chromium \
    && apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

EXPOSE 80
ENTRYPOINT ["dotnet", "iBudget.dll"]
