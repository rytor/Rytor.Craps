# Use the latest SQL Server image
FROM mcr.microsoft.com/mssql/server:latest

# Set the environment variables
ENV ACCEPT_EULA=Y
# Use ARG to pass the SA_PASSWORD value
ARG SA_PASSWORD
ENV SA_PASSWORD $SA_PASSWORD
ENV MSSQL_PID=Express

# Expose port 1433
EXPOSE 1433

# Mount volumes for data, log, and secrets
VOLUME /var/opt/mssql/data
VOLUME /var/opt/mssql/log
VOLUME /var/opt/mssql/secrets

# Copy the init.sh file into the container
COPY init.sh /usr/src/app/init.sh

# Run the init.sh script
CMD /bin/bash /usr/src/app/init.sh