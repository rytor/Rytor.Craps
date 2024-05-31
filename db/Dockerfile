# Use the official PostgreSQL image as a parent image
FROM postgres:latest

# Set the environment variable for the default superuser password
ARG POSTGRES_PASSWORD_ARG
ENV POSTGRES_PASSWORD=$POSTGRES_PASSWORD_ARG

# Set the environment variable for the default superuser name
ARG POSTGRES_USER_ARG
ENV POSTGRES_USER=$POSTGRES_USER_ARG

# Expose the PostgreSQL port
EXPOSE 5432

# Copy scripts to the container, to be run on postgres init
RUN mkdir /usr/scripts
RUN mkdir /usr/scripts/sql
COPY account.sql /docker-entrypoint-initdb.d/01_account_database_creation.sql
COPY account_ddl.sql /usr/scripts/sql/
COPY balance.sql /docker-entrypoint-initdb.d/02_balance_database_creation.sql
COPY balance_ddl.sql /usr/scripts/sql/
COPY balance_dml.sql /usr/scripts/sql/
COPY game.sql /docker-entrypoint-initdb.d/03_game_database_creation.sql
COPY game_ddl.sql /usr/scripts/sql/
COPY game_dml.sql /usr/scripts/sql/
COPY initialize_database.sh /docker-entrypoint-initdb.d/03_initialize_database.sh

CMD ["postgres"]
