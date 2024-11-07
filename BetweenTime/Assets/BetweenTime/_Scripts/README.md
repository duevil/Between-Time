# GameController API

## Table of contents

- [Overview](#overview)
- [GameController](#gamecontroller)
    - [Scripting](#scripting)
    - [Inspector](#inspector)
- [State Variables](#state-variables)
    - [Scripting](#scripting-1)
    - [Inspector](#inspector-1)
- [Debug Console](#debug-console)

## Overview

The game controller provides the variables for the different states of the game used to control the game flow.
The state variables are linked with a corresponding MQTT topic, being subscribed for external changes and publishing
their value when changing.

The GameController also handles the game's timer, starting and stopping it based on the current main state.

## GameController

### Scripting

The GameController uses a Singleton to provide an instance for use in other scripts:

```csharp
GameController.Instance
```

The instance can be used for accessing the game's states and the timer:

```csharp
GameController.Instance.mainState           // The game's main state
GameController.Instance.timecodeState       // The currently set timecode
GameController.Instance.candlesState        // The current state of the candles
GameController.Instance.mazePositionState   // The current position of the player in the maze
GameController.Instance.scannedItemsState   // The number of items scanned by the player
GameController.Instance.Running             // Whether the game is currently running
GameController.Instance.Timer               // The remaining time of the game's timer
```

### Inspector

The GameController also provides fields for the Unity inspector:

- `Mqtt Host`: The host address to use for the MQTT communication
- `Timer Duration`: The total duration for the game's timer
- `Timer`: The remaining time of the game's timer; **read-only**

Additionally, each state variable is shown in the inspector. For more details
see [State Variables: Inspector](#inspector-1).

## State Variables

The GameController provides five state variables:

- `mainState`: The game's main state
- `timecodeState`: The currently set timecode
- `candlesState`: The current state of the candles
- `mazePositionState`: The current position of the player in the maze
- `scannedItemsState`: The number of items scanned by the player

### Scripting

Each set provides a property `Value` that holds the current state's value in the corresponding type. For the `mainState`
`Value` can be set, for the other states `Value` is read-only and will only be updated via MQTT.

The states also provide an instance of a Unity event `onChange` that can be used to attach listeners to be called when
the state's `Value` was changed:

```csharp
GameController.Instance.mainState.onChange.AddListener(value => 
{
    // Do something with value ...
});
```

A listener can either be attached programmatically or using the `On Change` field inside the [inspector](#inspector-1).

The different states each hold a different value type:

- The `mainState` holds a value of the [`MainState` enum](base/MainState.cs)
- The `timecodeState` holds a simple integer representing the current timecode value
- The `candlesState` holds a value of the [`Candles` record](base/Candles.cs), which represents the state of each candle
  as a bitfield. The state for each candle [0,3] can be accessed using the Unity index operator:
  ```csharp
  // check if candle #2 is lit
  if (GameController.Instance.candlesState.Value[1]) {
      // Do something ...
  }
  ```
- The `mazePositionState` holds a value of [`MazePosition` record](base/MazePosition.cs), which holds the coordinates of
  a position inside the game's maze:
  ```csharp
  var x = GameController.Instance.mazePositionState.Value.X;
  var y = GameController.Instance.mazePositionState.Value.Y;
  // Do something with x and y ...
  ```
- The `scannedItemsState` currently only holds a plain integer value that has no further representation.

### Inspector

Each state is shown as a field of the GameController inside the Unity inspector and provides the following fields:

- `Topic`: The MQTT topic to link this state variable to
- `Value`: The state's current value; **read-only**
- `On Change`: The Unity event invoked when changing the state's value. This can be used to easily and directly link
  change listeners of other game objects to this state's value.

## Debug Console

Fo testing purposes, a custom debug console was implemented. The console can be opened during gameplay using the `F1`
key, and allows for the execution of custom commands.

The list of commands can be set inside the 