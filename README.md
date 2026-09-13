# UnoNoMercy# UnoNoMercy

A multiplayer card game engine (UNO with extended "No Mercy" rules) built in C# with a real-time backend using ASP.NET Core, SignalR, and a REST API.

> **Status:** Backend (game engine, API, real-time layer) is complete and functionally tested. A Flutter client is in early progress; console-based SignalR test clients are used for backend verification in the meantime.

## Overview

UnoNoMercy implements a full UNO-style multiplayer card game with several non-standard mechanics layered on top of the classic ruleset — including stacking draw penalties, an elimination "Mercy Rule," hand rotation, and a Wild Color Roulette card. Game actions are exposed over REST, with real-time state broadcasts and player session management handled via SignalR.

## Highlight: Session-Resumption Layer

The core technical challenge this project solves is **decoupling player identity from SignalR connection lifecycle**. A SignalR `ConnectionId` is transient — it changes on every reconnect — so a naive implementation breaks the moment a player's connection drops (network blip, app backgrounding, etc.).

This project implements a session-token layer that survives connection loss:
- Each player is issued a stable `SessionToken` on joining a room, independent of their live `ConnectionId`.
- A `ResumeSession` Hub method lets a dropped client reconnect with a *new* connection and reattach to their existing game session and identity.
- `OnDisconnectedAsync` carefully avoids tearing down a session if a newer connection has already taken over (guards against out-of-order disconnect events during fast reconnects).
- A background `SessionCleanupService` expires stale sessions after 24 hours of inactivity.

**This has been verified with two test scenarios**, not just assumed:
1. **Duplicate connection test** — confirms two separate connections for the same player get distinct connection IDs, and that disposing one doesn't affect the other's active session.
2. **Resume session test** — confirms a session token successfully reattaches to a brand-new connection after the original connection is fully disposed, simulating a real disconnect/reconnect cycle.

## Features

- Full turn-based game loop: draw, play, pass, with direction reversal and skip handling
- Draw-card stacking (Draw Two / Draw Four / Wild Draw Six / Wild Draw Ten) with penalty resolution
- Special card effects: Skip, Skip Everyone, Reverse, Discard All, Hand Swap (7), Hand Rotation (0)
- Wild Color Roulette — a custom mechanic where the active color is decided by repeated draws until a matching color appears
- "Mercy Rule" elimination when a player's hand exceeds a card threshold
- Session-based reconnection (see above) — players can drop and resume without losing their game state or identity
- Thread-safe game state mutation using per-game locking, supporting concurrent multiplayer sessions
- Deck rebuilding from the discard pile when the draw pile is exhausted
- Hybrid REST + SignalR architecture: game actions go through REST endpoints, with state changes broadcast in real time via SignalR groups

## Architecture

- **UnoNoMercy.GameEngine** — Core domain logic: `GameService`, `DeckService`, models, DTOs, enums. No external dependencies; pure game rules and state management.
- **UnoNoMercy.Api** — ASP.NET Core Web API + SignalR Hub (`GameHub`). Exposes game actions (`create`, `join`, `start`, `play-card`, `draw-card`, `pass-turn`, etc.) over REST, and handles connection/session lifecycle over SignalR.
- **UnoNoMercy.SignalRTest2 / SignalRtest3** — Console-based test harnesses verifying real-time SignalR behavior: duplicate connections, disconnect/reconnect, and session resumption.
- **UnoNoMercy.ConsoleTest** — Console-based client for manually driving and verifying game flow without a UI.
- **UnoNoMercy.ConcurrencyTest** — Tests around concurrent game state access.
- **uno_no_mercy_flutter** — Flutter client (in progress) — not yet functional end-to-end.

## Tech Stack

- C# / ASP.NET Core Web API
- SignalR (real-time communication, session management)
- Flutter (client — in progress)

## Current Limitations

- No persistent storage — game and session state are in-memory (single-instance only, won't survive an app restart or scale horizontally without rework).
- Flutter client is not yet wired up to the backend.
- No automated unit test suite yet — verification has been done via targeted console/SignalR test harnesses rather than xUnit/NUnit.

## Running the Project

```bash
git clone <repo-url>
cd UnoNoMercy

dotnet run --project UnoNoMercy.Api
```

Use `UnoNoMercy.ConsoleTest`, `UnoNoMercy.SignalRTest2`, or `UnoNoMercy.SignalRtest3` to simulate players and verify game flow, connection handling, and session resumption without the Flutter client.

## Roadmap

- [ ] Complete Flutter client integration
- [ ] Add automated unit tests (xUnit) for `GameService` rule logic and session handling
- [ ] Persistent game/session state (Redis or database-backed) to support horizontal scaling
- [ ] Thread-safe access to the game registry itself (currently guarded per-game, not at the dictionary level)
