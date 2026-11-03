# Water Sort

A mobile water sort puzzle game built in Unity (C#), played in portrait mode on Android.

![Gameplay](screenshots/gameplay.png)

## How to play
Tap a tube to select it, then tap another tube to pour. Only the top color
moves, and only onto an empty tube or a matching color with room left.
Sort the board until every tube holds a single color. Undo reverses a
pour, and Restart resets the level.

## Features
- 6 levels, from 3 colors with 2 empty tubes to 4 colors with 1 empty tube
- Undo with full board history, and level restart
- Animated pours
- Portrait mobile UI, with a camera that fits the board to any screen width
- Level counter and a win screen after the last level

## How it works
- Each tube is a stack of color ids, so the pour rule is a stack operation
- A GameManager handles selection, pour animation (coroutines),
  win detection and undo (a stack of board snapshots)
- Level data is stored as jagged arrays

## Download
Get the APK from the [https://github.com/turkanahmetoglu/water_sorting/releases/tag/v1.0] page. You may need
to allow installs from unknown sources on your phone.
