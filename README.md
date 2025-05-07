# Step Counter API

A company-wide steps leaderboard application for teams of employees. This application allows tracking step counts for teams and individual team members, providing a competitive and engaging way to promote physical activity in the workplace.

## Features

- Create and manage teams
- Create and manage step counters for team members
- Track step counts for individuals and teams
- View team rankings and total steps
- RESTful API design with proper error handling
- Input validation and consistent response models
- Swagger/OpenAPI documentation

## Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or later (recommended)
- Git

## Getting Started

1. Clone the repository:
```bash
git clone https://github.com/yourusername/StepCounter.git
cd StepCounter
```

2. Open the solution in Visual Studio or your preferred IDE

3. Build the solution:
```bash
dotnet build
```

4. Run the application:
```bash
dotnet run --project StepCounter.Api
```

The API will be available at `https://localhost:5001` and `http://localhost:5000`

## API Documentation

The API documentation is available through Swagger UI at `/swagger` when running the application.

### Teams API

#### GET /api/teams
Returns a list of all teams with their total step counts.

Response:
```json
[
  {
    "id": "guid",
    "name": "Team Name",
    "totalSteps": 1000
  }
]
```

#### POST /api/teams
Creates a new team.

Request:
```json
{
  "name": "Team Name"
}
```

Response:
```json
{
  "id": "guid",
  "name": "Team Name",
  "totalSteps": 0
}
```

#### DELETE /api/teams/{teamId}
Deletes a team.

#### GET /api/teams/{teamId}/steps
Gets the total steps for a team.

Response:
```json
{
  "id": "guid",
  "name": "Team Name",
  "totalSteps": 1000
}
```

#### GET /api/teams/{teamId}/counters
Gets all counters for a team.

Response:
```json
[
  {
    "id": "guid",
    "name": "Counter Name",
    "steps": 500
  }
]
```

### Counters API

#### POST /api/teams/{teamId}/counters
Creates a new counter for a team.

Request:
```json
{
  "name": "Counter Name"
}
```

Response:
```json
{
  "id": "guid",
  "name": "Counter Name",
  "steps": 0
}
```

#### DELETE /api/teams/{teamId}/counters/{counterId}
Deletes a counter from a team.

#### POST /api/teams/{teamId}/counters/{counterId}/increment
Increments the step count for a counter.

Request:
```json
{
  "steps": 100
}
```

Response:
```json
{
  "id": "guid",
  "name": "Counter Name",
  "steps": 600
}
```

## Error Responses

The API uses standard HTTP status codes and returns error responses in the following format:

```json
{
  "message": "Error message",
  "details": "Optional detailed error information",
  "errorCode": "Optional error code"
}
```

Common status codes:
- 200: Success
- 201: Created
- 204: No Content
- 400: Bad Request
- 404: Not Found
- 500: Internal Server Error

## Testing

The project includes unit tests. To run the tests:

```bash
dotnet test
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details. 