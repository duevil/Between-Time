# BETWEEN TIME

**'Between Time'** is a co-op puzzle game that challenges two players in separate dimensions - one using VR goggles in a virtual environment and one with access to a specially developed [physical controller](https://github.com/duevil/Between-Time-Controller) - to communicate and work together to solve a series of puzzles within a fixed time limit in a kind of escape room along the lines of *We Were Here* or *Keep Talking And Nobody Explodes*.

More details about the game can be found inside the [documentation (in German)](https://github.com/user-attachments/files/20357626/between-time.pdf).

## Setup Guide

**Unity version:** 6000.0.26f1

Before starting the game, the [physical controller](https://github.com/duevil/Between-Time-Controller) must first be set up. In addition, an MQTT broker (e.g. [Aedes](https://github.com/moscajs/aedes)) must be running to enable communication between the two systems/players. The host address of the broker is entered in the Unity Inspector of the GameController script. The port is set to 1883 by default.

## Miscellaneous

A statistic about the game's script files can be found [here](https://github.com/duevil/Between-Time/blob/BetweenTime/BetweenTime/Assets/BetweenTime/_Scripts/SCC.md).
