# GitIssueManager

**GitIssueManager** is a .NET 9 Web API for creating, updating, and closing issues across GitHub and GitLab using a unified, provider-agnostic interface.

---

## Features

- Unified issue management API for **GitHub** and **GitLab**
- Bearer token-based authentication support
- Auto-generated **Swagger** documentation
- Extensible service factory design
- Logging via **Serilog**

---

## Tech Stack

- ASP.NET Core 9
- Serilog (logging)
- Swashbuckle (Swagger)
- Newtonsoft.Json
- Moq + xUnit (testing)

---

## Project Structure

- GitIssueManager/
    - GitIssueManager.Api *- API layer*
    - GitIssueManager.Core *- Core logic and abstractions*
    - GitIssueManager.Tests *- Unit tests*

---

## Configuration

###  Providers Configuration

The `Providers` section in `appsettings.json` allows you to override the default API base URLs for **GitHub** and **GitLab**.

```json
"Providers": {
  "GitHub": {
    "BaseURL": "https://api.github.com"
  },
  "GitLab": {
    "BaseURL": "https://gitlab.com/api/v4"
  }
}
```

**Note:** This configuration is optional. If omitted, the following defaults will be used automatically:

* **GitHub:** `https://api.github.com`
* **GitLab:** `https://gitlab.com/api/v4`

You typically only need to customize this if you're pointing to a self-hosted or enterprise version of GitHub/GitLab.

### Serilog Logging

Logging is configured in `appsettings.json` under the `Serilog` section. By default, logs are written to the console and daily-rotating log files:

```json
"Serilog": {
  "MinimumLevel": {
    "Default": "Information",
    "Override": {
      "Microsoft": "Information",
      "System": "Warning"
    }
  },
  "WriteTo": [
    { "Name": "Console" },
    {
      "Name": "File",
      "Args": {
        "path": "Logs/log-.txt",
        "rollingInterval": "Day"
      }
    }
  ],
  "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ]
}
```

**More Info:**
See the official Serilog documentation for advanced scenarios:
[https://serilog.net](https://serilog.net)

---

## Running Locally

### Prerequisites

* .NET 9 SDK
* Docker (optional for containerized deployment)

### Steps

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run the API
dotnet run --project GitIssueManager.Api
```

Visit Swagger UI at:
**[https://localhost:7040/swagger](https://localhost:7040/swagger)**

---

## API Overview

| Endpoint             | Method  | Description              |
| -------------------- | ------- | ------------------------ |
| `/issues`            | `POST`  | Create a new issue       |
| `/issues/{id}`       | `PUT`   | Update an existing issue |
| `/issues/{id}/close` | `PATCH` | Close an issue           |

All API requests **require** an `Authorization` header with a **Bearer token**.

This token must be a **Personal Access Token (PAT)** from the respective provider (**GitHub** or **GitLab**), depending on the `provider` field in your request.

### Example Headers

* **For GitHub:**

  ```
  Authorization: Bearer ghp_yourgithubtoken123
  ```

* **For GitLab:**

  ```
  Authorization: Bearer glpat-yourgitlabtoken456
  ```

**Note:** Make sure your token has the necessary scopes/permissions to create and update issues in the target repository.

### 1. Create an Issue

**POST** `/issues`

Creates a new issue in the specified repository.

#### Request Body:

```json
{
  "provider": "GitHub",
  "repository": "username/repository",
  "title": "Bug: API not responding",
  "description": "The API returns a 500 error when..."
}
```

---

### 2. Update an Issue

**PUT** `/issues/{id}`

Updates the title or description of an existing issue.

#### Request Body:

```json
{
  "provider": "GitLab",
  "repository": "mygroup/myproject",
  "title": "Bug: API timeout fixed",
  "description": "Resolved by increasing timeout settings."
}
```

---

### 3. Close an Issue

**PATCH** `/issues/{id}/close`

Closes an open issue in the specified repository.

#### Request Body:

```json
{
  "provider": "GitHub",
  "repository": "username/repository"
}
```

---

## Testing

To run the test suite:

```bash
dotnet test
```

Tests are located in the `GitIssueManager.Tests` project and cover service logic.
