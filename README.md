## Prerequisites

- Redis
- PostgreSQL
- Docker
- DotNet 8
- 
## Installation

1. Install Docker Desktop https://www.docker.com/products/docker-desktop
2. Create Redis container "docker run --name redis -p 6380:6379 -e REDIS_USER=user -e REDIS_PASSWORD=default -e REDIS_USER_PASSWORD=password -d redis:latest" in Docker
3. Create PostgreSQL container "docker run --name container-name -p 5432:5432 -e POSTGRES_USER=user -e POSTGRES_PASSWORD=password -e POSTGRES_DB=database -d postgres:17.2" in Docker
4. Install DotNet 8 SDK https://dotnet.microsoft.com/en-us/download/dotnet/8.0
5. 
## Migration

Use "dotnet ef database update" on the Domain project
