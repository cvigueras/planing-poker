# Planing Poker

Real-time Planning Poker web app. An anonymous user creates a game, shares its unique Id, and the rest of the team joins, votes and reveals all votes together.

## Tech stack

| Layer | Technologies |
|-------|--------------|
| Backend (`PlaningPoker.Api`) | .NET 10, ASP.NET Core Web API, SignalR, MediatR (CQRS), AutoMapper, Dapper, SQLite, Swagger |
| Frontend (`PlaningPoker.App`) | Vue 3, Vuex, Vue Router, Axios, `@aspnet/signalr` (Vue CLI 5) |
| Tests (`PlaningPoker.Api.Test`) | NUnit, FluentAssertions, NSubstitute, AutoFixture, Verify, `Microsoft.AspNetCore.Mvc.Testing` |

## Project structure

```
PlaningPoker.sln
├── PlaningPoker.Api        # Web API + SignalR hub (features: Cards, Games, Users, Votes)
├── PlaningPoker.Api.Test   # Unit, repository, feature and SignalR hub tests
└── PlaningPoker.App        # Vue 3 SPA
```

Each API feature folder is organised as `Controllers / Commands / Queries / Models / Repositories`.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js](https://nodejs.org/) and npm
- ASP.NET Core HTTPS development certificate (see [HTTPS certificate](#https-certificate))

## Getting started

### 1. Run the API

```bash
cd PlaningPoker.Api
dotnet run --launch-profile https
```

- API: `https://localhost:7096` (also `http://localhost:5083`)
- Swagger UI: `https://localhost:7096/swagger`
- SignalR hub: `https://localhost:7096/hubs/planing`

The SQLite database (`PlaningPoker.db`) and its tables (`Games`, `Users`, `Cards`) are created and seeded automatically on startup.

### 2. Run the frontend

```bash
cd PlaningPoker.App
npm install
npm run serve
```

The app is served at `https://localhost:5002`. The dev server proxies `/cards`, `/game`, `/votes` and `/users` to the API.

### HTTPS certificate

`vue.config.js` reads the dev certificate from `%APPDATA%/ASP.NET/https` (Windows) or `~/.aspnet/https` (macOS/Linux), using the name `vueapp` by default (override with `--name=<name>`). If it does not exist, create it with:

```bash
dotnet dev-certs https --export-path ~/.aspnet/https/vueapp.pem --format Pem --no-password
```

## Configuration

### Frontend (`PlaningPoker.App/.env`, `.env.production`)

| Variable | Description | Development value |
|----------|-------------|-------------------|
| `VUE_APP_ENVIROMENT` | API base URL (target of the dev proxy; in production the base URL of the requests) | `https://localhost:7096/` |
| `VUE_APP_REDIRECT_PORT` | Port of the Vue dev server | `5002` |
| `VUE_APP_ENVIROMENT_SIGNALR` | SignalR hub URL | `https://localhost:7096/hubs/planing` |
| `VUE_APP_PUBLIC_PATH` | Folder the app is served from (only in `.env.production`; `/` when not set) | — |

`.env.production` holds the production values: the API hosted on Fly.io and `/planing-poker/` as public path.

### Backend (`PlaningPoker.Api/appsettings.json`)

| Key | Description |
|-----|-------------|
| `Enviroments:ClientPolicyUrl` | Origin allowed by CORS (the frontend URL, `https://localhost:5002` by default). |

## Deployment

### Web (GitHub Pages)

The workflow `.github/workflows/deploy-web.yml` builds the Vue app and publishes it at
`https://<user>.github.io/planing-poker/` on every push to `main` that changes `PlaningPoker.App/` (it can also be
run by hand from the Actions tab).

One-time setup: in the repository, **Settings > Pages > Build and deployment > Source: GitHub Actions**.

The build copies `index.html` to `404.html`, so routes such as `/planing` still work when the page is refreshed.

### API (Fly.io)

The web talks to the API set in `.env.production`. The API is not deployed by the workflow; when it changes, deploy it
again so it matches the web (for example the SignalR hub methods the page calls). In the API:

- Set `Enviroments__ClientPolicyUrl` to the origin of the web, without path: `https://<user>.github.io`.
- The database is a SQLite file (`PlaningPoker.db`), so keep it on a persistent volume or every deployment starts
  with an empty database.

## API reference

### REST

| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/cards` | List all card values. |
| `POST` | `/game` | Create a game; returns its unique Guid. |
| `GET` | `/game/{guid}` | Get a game with its users and card values. |
| `PUT` | `/game/{guid}` | Add a user to an existing game. |
| `GET` | `/game/GetNumberMatchs` | Total number of games created. |
| `GET` | `/users/GetTotalNumberUsers` | Total number of users. |
| `POST` | `/votes` | Register a user's vote. |

### SignalR hub (`/hubs/planing`)

Used to keep every user in the same game in sync: joining/leaving a game (`JoinGroup`, `RemoveFromGroup`), publishing users (`PublishUser`), notifying that someone has voted (`NotifyUserHasVoted`), revealing all votes (`ReceiveAllVotes`) and sending messages (`SendMessageToGroup`, `SendMessageToUser`, `SendMessageToAll`, `SendMessageToCaller`).

## Frontend routes

| Route | View |
|-------|------|
| `/` | Home: create or join a game. |
| `/planing` | Game room: card list, player panel and votes. |
| any other | Page not found. |

## Tests

```bash
dotnet test
```

## License

See [LICENSE](LICENSE).
