Nullam is an app to manage social events. User can:
- create events
- delete future events
- add participants to events
- view full list of participants
- remove participants from events
- edit registered participants
- manage payment types
- already registered participant can be added to other events via search function built into first name field.

App frontend is built with React and backend API with .NET 7. Data is stored in Microsoft SQL database.
To try it out you can utilize Docker containers.



## Setup with Docker
Requirements:
- Docker Engine 19.03.0 or higher
- Docker Compose file format 3.8 or higher


In ```Nullam/``` directory where ```docker-compose.yml``` file is, run:
```bash
docker-compose up --build -d 
```
This will create 3 containers and app is ready to use.
- ```db-server```
  - ```db: nullam```
  - ```username: sa```
  - ```password: Passw0rd1```
- ```web-api```
  - http://localhost:8000/swagger/index.html
- ```web-ui```
  - http://localhost:3000/

## Setup manually
Requirements:
- MSSQL LocalDB or Microsoft SQL Server Express (recent stable version recommended)
- .NET 7.0 or higher
- C#11 or higher
- Node 18.17.1 or higher
- yarn 1.22.21 or higher

Open solution in ```Nullam/backend/Nullam.sln``` and run app from IDE.  

Or in ```Nullam/backend/API``` run:
```bash
dotnet run
```

Next in ```Nullam/frontend/react-web``` to install dependencies, run:
```bash
yarn
```
and then to run the app:
```bash
yarn dev
```
After these steps app is ready to use
- ```db-server```
  - ```(localdb)\\MSSQLLocalDB```
  - ```Windows authentication```
- ```web-api```
  - http://localhost:5145/swagger/index.html
- ```web-ui```
  - http://localhost:5173