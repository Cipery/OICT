# .net dev zkušební úkol

Spuštění aplikace v docker konteinerech pomocí docker compose  
Doporučuji použít Windows Powershell. Skripty se nacházejí v kořenovém adresáři projektu  
```sh
./run-docker-compose.bat
```
Pro spuštění integračních testů v docker konteinerech  
```sh
./run-integration-tests.bat
```

Swagger je k dispozici na http://localhost:45000/swagger/index.html  

Integrační testy, stejně tak jako další testy, lze spustit i pomocí ```dotnet test``` v adresáři projektu testů nebo přes visual studio - run unit tests