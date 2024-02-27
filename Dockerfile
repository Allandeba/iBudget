ARG VERSION=7.0
ARG TAG=$VERSION-alpine

FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build
WORKDIR /app

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c release -o out --no-restore --no-cache /restore

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

FROM mcr.microsoft.com/dotnet/aspnet:$TAG
WORKDIR /app

# Fix InvariantCulture when using Alpine version
RUN apk add --no-cache icu-libs icu-data-full
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=build /app/out .

EXPOSE 80 443

ENTRYPOINT ["dotnet", "iBudget.dll"]