FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-stage
WORKDIR /app
COPY . ./
RUN dotnet restore && dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build-stage /app/out .

RUN apt-get update && apt-get install -y wget gnupg && \
    wget -qO - https://www.mongodb.org/static/pgp/server-7.0.asc | gpg --dearmor -o /usr/share/keyrings/mongodb.gpg && \
    echo "deb [signed-by=/usr/share/keyrings/mongodb.gpg] http://repo.mongodb.org/apt/debian bookworm/mongodb-org/7.0 main" > /etc/apt/sources.list.d/mongodb-org-7.0.list && \
    apt-get update && apt-get install -y mongodb-mongosh && rm -rf /var/lib/apt/lists/*

COPY Data/e-commerce /Data/e-commerce

EXPOSE 8080
ENV ASPNETCORE_URLS="http://+:8080"

CMD ["sh", "-c", "\
  echo 'Waiting for MongoDB...'; \
  until mongosh --host mongo-1 --port 27017 --username admin --password password --eval 'db.adminCommand({ ping: 1 })'; do \
    echo 'MongoDB not available, retrying in 2 seconds...'; \
    sleep 2; \
  done; \
  echo 'Restoring data...'; \
  mongorestore --host mongo-1 --port 27017 --username admin --password password --authenticationDatabase admin --db e_commerce_db /Data/e-commerce; \
  echo 'Starting application...'; \
  exec dotnet E_commerce_app_dotnet.dll"]