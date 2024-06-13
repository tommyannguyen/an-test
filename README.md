# Requiment

Make sure dotnet 8 be installed 

# Run application

## 1. Run application
cd AnNguyen.Spa\AnNguyen.Spa.Server

dotnet run dotnet run 

## 2. Browser

Load this url
https://localhost:5173/



# Run in Docker
## Note : FE and Api Service can separated into 2 nodes. Api Services can setup muptiple intances

docker compose build
docker compose up 

Browser load
https://localhost:9000/

Default ports range from 9000-9010
## Run docker compose multiple intances
docker-compose.yml

Please adjust number of intances by set replicas 