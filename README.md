# Requirement

Make sure dotnet 8 be installed 

# Run application

## 1. Run application
cd AnNguyen.Spa\AnNguyen.Spa.Server

dotnet run dotnet run 

## 2. Load web

Load this url
https://localhost:5173/



## 3. Run in Docker
### Note : FE and Api Service can separated into 2 nodes. Api Services can setup muptiple intances

docker compose build
docker compose up 

Browser load
https://localhost:9000/

Default ports range from 9000-9010
### Run docker compose multiple intances
docker-compose.yml

Please adjust number of intances by set replicas 


### Example
![image](https://github.com/tommyannguyen/an-test/assets/5110596/5b364412-8ab6-4ca5-848b-ec972d9035d3)

![image](https://github.com/tommyannguyen/an-test/assets/5110596/a930f954-7973-46f6-b2c2-41845d364dfb)


