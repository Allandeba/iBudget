#!/bin/sh
psql --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" \
    --set=ON_ERROR_STOP=1 --set=app_user="$IBUDGET_DB_USER" \
    --set=app_password="$IBUDGET_DB_PASSWORD" --set=db_name="$POSTGRES_DB" <<'SQL'
CREATE ROLE :"app_user" LOGIN PASSWORD :'app_password';
ALTER DATABASE :"db_name" OWNER TO :"app_user";
ALTER SCHEMA public OWNER TO :"app_user";
CREATE EXTENSION IF NOT EXISTS unaccent WITH SCHEMA public;
SQL
