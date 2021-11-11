# Dakar Rally API

## About the application
Dakar Rally application represents real-time simulation of a race. When DakarRally is started, DakarRally.API.exe and swagger are started. DakarRally.API.exe is a worker service which handles simulation of a race and swagger is used for testing endpoints.

## Tool for testing
- Swagger

## Steps to reproduce for starting race simulation
1. Create race using POST /Race endpoint and copy the given raceId
2. Use raceId for POST /Vehicle endpoint to create as many vehicles for race as you want
3. To start a race, use raceId for PATCH /Race/{raceId} endpoint 
4. Use raceId to see leaderboard while the race is running with GET /Leaderboard endpoint
 
## Tech decisions
- [Web API written in .Net Core 5.0](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-5.0)
- [AutoMapper](https://github.com/AutoMapper/AutoMapper)

## Project structure
- DakarRally.API - contains Controllers, AutoMapper and Swagger
- DakarRally.Infrastructure.DbContext - contains DBContext
- DakarRally.Infrastructure.Repositories - contains actions executed against database
- DakarRally.Core.DTO - contains objects needed for CRU actions
- DakarRally.Core.Entities - contains models, enums and constants
- DakarRally.Core.Services - contains services for communication with repositories and worker service which represents race simulation
- DakarRally.Tests - contains unit tests 

## Points for improvements
- Add unit tests for all layeres
- Adapt swagger for better scheme understanding
