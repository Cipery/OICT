docker-compose  -f "./docker-compose.yml" -f "./docker-compose.override.yml" -f "./docker-compose.integration.yml" --no-ansi up --force-recreate --remove-orphans --build integration 
pause