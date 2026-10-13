ARG ARCH=amd64
ARG VERSION=8.0
# Ubuntu 22.04 retains the native dependencies required by Syncfusion Blink.
ARG TAG=$VERSION-jammy-$ARCH

FROM mcr.microsoft.com/dotnet/sdk:$VERSION-jammy AS build
WORKDIR /app

COPY src/iBudget.csproj ./src/
COPY Shared/Shared/Shared.csproj ./Shared/Shared/
RUN dotnet restore src/iBudget.csproj

COPY src/ ./src/
COPY Shared/ ./Shared/
RUN dotnet publish src/iBudget.csproj -c Release -o /app/out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:$TAG
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=80

# Syncfusion
RUN apt-get update \
    && apt-get install -yq --no-install-recommends \
        libasound2 libatk1.0-0 libc6 libcairo2 libcups2 libdbus-1-3 \
        libexpat1 libfontconfig1 libgcc1 libgconf-2-4 libgdk-pixbuf2.0-0 libglib2.0-0 libgtk-3-0 libnspr4 \
        libpango-1.0-0 libpangocairo-1.0-0 libstdc++6 libx11-6 libx11-xcb1 libxcb1 \
        libxcursor1 libxdamage1 libxext6 libxfixes3 libxi6 libxrandr2 libxrender1 libxss1 libxtst6 \
        libnss3 libgbm1 \
    && apt-get autoclean \
    && apt-get autoremove -y \
    && rm -rf /var/cache/apk/* \
    && rm -rf /var/lib/{apt,dpkg,cache,log}/*

COPY --from=build /app/out .

EXPOSE 80 443

ENTRYPOINT ["dotnet", "iBudget.dll"]
