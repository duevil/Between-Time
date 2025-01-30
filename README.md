# BETWEEN TIME

**‘Between Time’** ist ein Koop-Rätsel-Spiel, welches in einer Art Escape-Room nach Vorbildern wie *We Were Here* oder *Keep Talking And Nobody Explodes* zwei Spieler in getrennten Dimensionen — einer mittels VR-Brille in einer virtuellen Umgebung und einer mit Zugriff auf einen eigens entwickelten physischen Controller — zur Kommunikation und Zusammenarbeit herausfordert, um innerhalb eines festen Zeitlimits eine Reihe von Puzzles zu lösen.

## Anleitung

**Unity-Version:** 6000.0.26f1

Vor dem Starten des Spiels muss zunächst der [physische Controller](https://git.hs-harz.de/xrws24/xrws24/-/tree/BetweenTime-hardware) eingerichtet werden. Zudem muss ein MQTT-Broker (z.B. [Aedes](https://github.com/moscajs/aedes)) laufen, um die Kommunikation zwischen den beiden Systemen/Spielern zu ermöglichen. Die Host-Adresse des Brokers wird im Unity-Inspector des GameController-Skripts eingetragen. Der Port ist standardmäßig auf 1883 gesetzt.

Nähere Details zum Spielablauf und den Puzzles sowie der Implementierung sind in der [Dokumentation]() zu finden.