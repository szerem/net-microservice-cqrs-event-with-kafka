# openshift-for-developers

## cerate docker network
```
docker --version  
docker network create --attachable -d bridge mydockernetwork 
docker network ls
 ```

## install dockers images 
```
docker-compose --version

docker-compose --file .\docker\docker-compose.yml up -d
or in docker folder 
docker-compose up -d


docker run -it -d --name mongo-container -p 27017:27017 --network mydockernetwork --restart always -v mongodb_data_container:/c/data mongo:latest


docker run -d --name sql-container --network mydockernetwork --restart always -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=$tr0ngS@P@ssw0rd02' -e 'MSSQL_PID=Express' -p 1433:1433 mcr.microsoft.com/mssql/server:2017-latest-ubuntu 
if port unavailable
netstat -ano | findstr "PID: 1433"
& 'C:\Program Files\Docker\Docker\DockerCli.exe' -SwitchDaemon;
net stop winnat;
docker start sql-container;
docker start mongo-container;
net start winnat;

```
<!-- docker compose start -->

choco install robo3t.portable
dotnet .\SM-Post\Post.Cmd\Post.Cmd.Api\bin\Debug\net6.0\Post.Cmd.Api.dll --environment=MyEnvironment 




