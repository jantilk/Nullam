## Setup database
in Nullam/backend directory run
```bash
dotnet ef database update --project Persistence --startup-project API
```

## Setup Docker containers
in repo root directory where docker-compose.yml file is
```bash
docker-compose up --build -d 
```
