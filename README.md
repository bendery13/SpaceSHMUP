# SpaceSHMUP

## Course Information
**Southern Illinois University Edwardsville**
<br>**Course:** CS 382 - Game Design, Development, and Technology
<br>**Authors:** Ryan Bender and Jacob Stephens

---

## Project Overview

SpaceSHMUP is a vertical-scrolling space shooter where the player pilots a hero ship against waves of enemies with increasingly complex behaviors. Survive as long as possible, collect power-ups to upgrade your weapons and shields, and climb the high score leaderboard. The game can be played through Unity Hub here: [Space SHMUP Game](https://play.unity.com/en/games/bda9948e-5f68-43d1-a826-5e2ada60e5a9/space-shmup)

The game demonstrates foundational Unity concepts, including:

- Physics-based collision detection and projectile mechanics
- Flexible weapon and power-up systems driven by data definitions
- Enemy AI with varied movement patterns (linear, sinusoidal, Bézier curves, random interpolation)
- Shield systems for both the player and elite enemies
- Parallax scrolling background with player-reactive motion
- Persistent high score tracking with `PlayerPrefs`
- Lives system with mid-run respawning
- Main Menu, Controls Screen, and Game Over Screen with leaderboard display

---

## Added Element — "To Make the Game Cooler in a Meaningful Way"

To make the game cooler in a meaningful way, we focused on giving players better information at every stage of a run — during gameplay, at death, and across sessions — because information is what turns a passive experience into an intentional one.

During gameplay, a **live score counter** and a **lives counter** are always visible on the HUD. The score updates in real time as enemies are destroyed, rewarding higher-value targets like Enemy_4 with more points than basic enemies, which gives players a reason to prioritize dangerous ships instead of just dodging them. The lives counter works alongside this by raising the stakes of every decision, when a player can see they are down to their last life, every enemy and every movement carries real weight. Together, these two elements transform the HUD from a status display into a source of constant tension and motivation that keeps players engaged from the first enemy to the last.

When a run ends, that tension carries over into the **persistent top-8 high score leaderboard** shown on the Game Over screen, stored via `PlayerPrefs`. A single saved score gives a player one number to beat; a leaderboard gives them eight, ranked by their best performances across every session. Players can see exactly how far their latest run fell short, which run was their personal best, and how much room they have to improve. This turns the Game Over screen from a dead end into a starting line — the kind of feedback loop that makes players want to immediately try again.


---

## Gameplay

### How to Play

- Use **Arrow Keys** or **WASD** to move the hero ship.
- Press **Spacebar** to fire your current weapon.
- Collect **power-up cubes** dropped by destroyed enemies to upgrade weapons or restore shields.
- Survive incoming enemy waves and destroy as many enemies as possible.
- The game ends when all lives are exhausted. Your score is submitted to the leaderboard.

### Weapons

| Type    | Description                                      |
|---------|--------------------------------------------------|
| Blaster | Default single shot, fires straight up           |
| Spread  | Fires two simultaneous angled shots              |
| Shield  | Raises the hero's shield level instead of firing |

### Power-Ups

Destroyed enemies have a chance to drop a rotating colored cube power-up. Flying into it applies its effect (weapon upgrade or shield restore). Power-ups fade out and disappear if not collected in time.

### Shield System

The hero ship has a shield with multiple levels displayed as a rotating overlay. Taking damage from an enemy reduces shield level. Collecting a Shield power-up raises it back up. At shield level 0, the ship is destroyed and a life is lost.

---

## Enemy Types

| Enemy   | Points | Movement Pattern                                                                 |
|---------|--------|----------------------------------------------------------------------------------|
| Enemy_0 | 50     | Moves straight down at a constant speed                                          |
| Enemy_1 | 100    | Moves down with a sinusoidal side-to-side weave                                  |
| Enemy_2 | 150    | Moves down with an accelerating sinusoidal weave                                 |
| Enemy_3 | 200    | Follows a random Bézier curve path across the screen                             |
| Enemy_4 | 350    | Interpolates between random screen positions; protected by a destructible shield |

---

## Lives & Scoring

- The player starts with **3 lives**.
- On death, the scene reloads with the remaining life count and score preserved.
- When all lives are gone, the **Game Over screen** appears showing the final score and the top-8 leaderboard.
- High scores persist between sessions via `PlayerPrefs`.

---

## Screens

| Screen         | Description                                                                 |
|----------------|-----------------------------------------------------------------------------|
| Main Menu      | Entry point with Start and Controls options                                 |
| Controls Menu  | Displays the control scheme for new players                                 |
| Game           | The main gameplay scene with HUD showing score, high score, and lives       |
| Game Over      | Displays final score and the top-8 persistent leaderboard; Restart or Menu  |
