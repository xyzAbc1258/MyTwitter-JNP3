FROM microsoft/mssql-server-linux

ENV SA_PASSWORD=1qaz2WSX
ENV ACCEPT_EULA=Y

HEALTHCHECK --interval=10s  \
	CMD /opt/mssql-tools/bin/sqlcmd -S . -U sa -P 1qaz2WSX \
-Q "CREATE DATABASE [MyTwitter] ON (FILENAME = '/var/opt/sqlserver/MyTwitter.mdf'),(FILENAME = '/var/opt/sqlserver/MyTwitter_log.ldf') FOR ATTACH"