# BETWEEN TIME

**'Between Time'** is a co-op puzzle game that challenges two players in separate dimensions - one using VR goggles in a virtual environment and one with access to a specially developed [physical controller](https://github.com/duevil/Between-Time-Controller) - to communicate and work together to solve a series of puzzles within a fixed time limit in a kind of escape room along the lines of *We Were Here* or *Keep Talking And Nobody Explodes*.

More details about the game can be found below (in German).

## Setup Guide

**Unity version:** 6000.0.26f1

Before starting the game, the [physical controller](https://github.com/duevil/Between-Time-Controller) must first be set up. In addition, an MQTT broker (e.g. [Aedes](https://github.com/moscajs/aedes)) must be running to enable communication between the two systems/players. The host address of the broker is entered in the Unity Inspector of the GameController script. The port is set to 1883 by default.

## Miscellaneous

A statistic about the game's script files can be found [here](https://github.com/duevil/Between-Time/blob/BetweenTime/BetweenTime/Assets/BetweenTime/_Scripts/SCC.md).

## Dokumentation

### Einführung

#### Überblick

![between-time](https://github.com/user-attachments/assets/35352635-deab-4943-94d2-5987c195ddc7)

**‘Between Time’** ist ein Koop-Rätsel-Spiel, welches in einer Art Escape-Room nach Vorbildern wie *We Were Here* oder *Keep Talking And Nobody Explodes* zwei Spieler in getrennten Dimensionen – einer mittels VR-Brille in einer virtuellen Umgebung und einer mit Zugriff auf einen eigens entwickelten physischen Controller – zur Kommunikation und Zusammenarbeit herausfordert, um innerhalb eines festen Zeitlimits eine Reihe von Puzzles zu lösen.

#### Story

Zeit ist instabil. Zeitrisse tauchen immer wieder auf und lassen die verschiedenen Zeitstrahle des Universums miteinander verschmelzen. Dabei entsteht ein Ort zwischen den Zeiten, in dem sich Gegenstände aus den verschiedensten Zeiten ansammeln. Jedes Mal, wenn so ein Ort entsteht, muss ein spezialisierter Agent hineingeschickt werden, um mit Hilfe von außen den Riss zu schließen und einen Zusammenbruch des Raum-Zeit-Kontinuums zu verhindern. Dabei muss der Agent für jede betroffene Zeit den entsprechenden Zeitkern finden, bergen und wieder in seine ursprüngliche Fassung setzen, um die Zeit zu stabilisieren. Der Agent außerhalb des Zeitrisses hat über ein spezielles Gerät, welches sich mit den betroffenen Zeiten im Zeitriss verbinden kann, Zugriff auf wichtige Informationen aus den unterschiedlichen Zeiten, welche dem Agenten im Zeitriss helfen, die Zeitkerne zu bergen. Nur durch die richtige Zusammenarbeit beider Agenten kann der Zeitriss geschlossen werden und das Raum-Zeit-Kontinuum weiter bestehen.

#### Spielablauf & Rätsel

Um das Spiel zu starten, muss zunächst die physische Rätselbox eingerichtet werden. Dazu kann die Anleitung in der README des Repositories der Rätselbox befolgt werden. Sind die Rätselbox und der Rechner, auf welchem die Unity-Anwendung läuft, im selben Netzwerk, kann ein MQTT-Broker wie Aedes genutzt werden, um die beiden “Spieler” miteinander zu verbinden (die MQTT-Adresse in Unity kann über die GameController-Klasse eingestellt werden).

Gestartet wird das Spiel über den Controller in der virtuellen Umgebung, welcher das Gegenstück zur physischen Rätselbox darstellt. Der virtuelle Spieler kann diesen greifen und über den Trigger des VR-Controllers den Spielstart einleiten. Dabei werden alle vorherigen Werte zurückgesetzt. Die Spieler müssen nun gemeinsam die drei Rätsel lösen, wobei am Ende jedes Rätsels ein ‘Timecore’ erreicht wird, welcher in seinen entsprechenden Sockel gesetzt werden muss, wodurch das Spiel gewonnen ist. Dafür ist den Spieler jedoch ein Zeitlimit von 10 Minuten gesetzt; sollten sie die Timecore nicht vor Ablauf der Zeit platzieren können, ist das Spiel verloren.

Zu jeder Zeit kann nur eines der Rätsel bearbeitet werden, dies geschieht in einer festen Reihenfolge. Um mit einem Rätsel zu interagieren, muss zuvor ein vom physischen Spieler der Timecode gesetzt werden, um auf die entsprechende Zeitlinie zu “synchronisieren”.


##### Rätsel 1: Maya-Tempel

![between-time-2](https://github.com/user-attachments/assets/03f52abc-5d1d-4129-9162-75e6b2407ccb)

Das erste Rätsel ist ein kleiner Tempel, der an alte Maya-Pyramiden angelehnt ist. Die Lösung dieses Rätsels sieht wie folgt aus:

- Synchronisation auf dem Timecode dc3e16. Der Code kann auf der linken Seite des Tempels abgelesen werden.
- Umlegen des Schalters des physischen Controllers, welcher auf der im virtuellen Raum zu findenden Steintafel gekennzeichnet ist. Dies aktiviert ein Eingabefeld mit Symbolen.
- Eingabe durch den virtuellen Spieler der Abfolge der Symbole, welche auf dem physischen Display angezeigt werden.
- Wurde der korrekte Code angezeigt, wird der Timecore freigegeben und das Eingabefeld zeigt eine Kombination an, welche für das nächste Rätsel benötigt wird.

##### Rätsel 2: Mittelalter-Altar

![between-time-3](https://github.com/user-attachments/assets/6ed015e2-c907-4b39-a705-f77edb928a4d)

Das zweite Rätsel ist ein kleiner Alter, welches sich an die Epoche des Mittelalters anlehnen soll. Das Puzzle wie folgt gelöst:

- Synchronisation auf dem Timecode 400116. Der Code kann auf der linken Seite der verschlossenen Truhe abgelesen werden.
- Platzieren der korrekten Kerzen in einem der Kerzenhalter auf dem Altar. Jede Kerze hat ein Symbol auf der Unterseite, wobei vier entsprechende Symbole zuvor auf dem Maya-Tempel markiert worden sind.
- Anzünden der Kerzen in der richtigen Reihenfolge. Die korrekte Reihenfolge erscheint als Farben auf dem Buch im virtuellen Raum und muss auf die Schalter des physischen Controllers übertragen werden.
- Eingabe eines Binärcodes. Dieser erscheint auf der zweiten Buchseite als römische Zahl, welche mittels der abgedruckten Hilfestellung als Binärdarstellung über die physischen Schalter eingegeben werden muss.
- Die Truhe neben dem Altar öffnet sich und gibt den Timecore sowie ein kupfernes Zahnrad preis.

##### Rätsel 3: Steampunk-Labyrinth

![between-time-4](https://github.com/user-attachments/assets/50bf27bb-4b2b-4498-92f7-cf4059e33665)

Das dritte und letzte Rätsel erinnert an das fiktive Genre des Steampunk oder der Epoche der Industrialisierung. Gelöst wird das Rätsel folgendermaßen:

- Synchronisation auf dem Timecode 14ea16. Der Code befindet sich auf der Oberseite des Sockels der Maschine.
- Aktivierung der Maschine mittels Platzieren des Zahnrades. Nach der Synchronisation öffnet sich eine Klappe an der Maschine, in welche das Zahnrad aus der Mittelalter-Truhe eingesetzt werden kann.
- Bewältigung des Labyrinths. Der virtuelle Spieler muss dem physischen Spieler das Layout des Labyrinths übermitteln, während der physische Spieler über die Schalter seines Controllers den geschrumpften Timecore durchs Labyrinth bewegen kann.
- Wird das Ende des Labyrinths erreicht, kann der Timecore gegriffen werden.

##### Spielende

![between-time-1](https://github.com/user-attachments/assets/01c5d5ac-769a-44c1-8505-cd43a6109ed6)

Zuletzt (oder zwischendrin) muss der virtuelle Spieler die drei Timecores in ihren jeweils passenden Sockel platzieren. Passiert dies innerhalb des Zeitlimits, ertönt eine Siegesfanfare und das Spiel ist gewonnen. Andernfalls ist das Spiel verloren. Nach Ende des Spiels – d. h. Sieg oder Niederlage gegen die Zeit – kann mittels Interaktion mit dem virtuellen Controller das Spiel zurückgesetzt und ein neuer Versuch gestartet werden.

### Komponenten

#### Grundlegende Struktur

##### GameController und MQTT

Kern der Spiel-Architektur ist die GameController-Klasse. Diese verwaltet den grundlegenden Spielablauf sowie die MQTT-Kommunikation und die unterschiedlichen Rätsel-Status. Zum einfachen Empfangen von durch den physischen Controller gesendeten Werten und dem Senden eigener Aktualisierungen wird eine eigene State-Klasse genutzt, welche ein Interface für die MQTT-Werte bietet.

##### Rätsel-GameObjects

Die Logik von Spieler-Eingaben sowie die Animation und Verwaltung von interaktiven Elementen wird wiederum von eigenen Rätsel-GameObjects gehandhabt, welche in eigenen Script jeweils die jeweiligen Abläufe implementieren und mit den Werten des GameControllers kommunizieren.

##### Kommunikation zwischen Skripten

Weitreichende Kommunikation zwischen Skripten – primär also zwischen den Rätsel-GameObjects und dem GameController, aber auch in anderen Fällen wie bspw. Sound-Trigger – erfolgt über das Unity-Event-System. Der GameController erlaubt das Hinzufügen von Event-Handlern zu jeder State-Variable. Die Rätsel-Scripts überwachen ihren Status nicht selbst, sondern hängen sich nur an die eigenen State-Variablen dran und updaten diese, wodurch die Event-Chain ausgelöst wird. Dadurch können flexibel unterschiedliche Systeme an einzelne Status-Werte geknüpft und auf ständige Überwachung oder das Setzen diverser Flag-Variablen verzichtet werden.

##### Weitere Skripte

Neben den primären Skripten wurden einige kleinere Skripte geschrieben, welche kleinere, meist rein kosmetische (d. h. nicht für die Funktion des Spiels essenzielle) Funktionen implementieren, wie bspw. das Zurück-Teleportieren von Spieler oder geworfenen Objekten zur Ursprungsposition, sollten sich Spieler oder Objekte zu weit von dieser entfernen.

##### Animation

Bis auf ein paar Ausnahmen wird für die Animation der meisten Spiel-Elemente die eigens geschriebene generische SmoothLerpAnimation-Klasse genutzt. Diese bietet ein einfaches Script-based Animations-Interface auf Grundlage der Unity-Lerp-Klassen und erlaubt zudem leichtes Aufbauen von
verketteten Animations-Abläufen.

#### Darstellung im Diagramm

![between-time pdf-image-005](https://github.com/user-attachments/assets/c7e535fa-c895-48ac-b379-62e59e424759)

Der GameController enthält State-Variablen, wie MainState, Timecode, MazePosition und Candles. Wenn sich State-Variablen verändern, werden die Rätsel-GameObjects bzw. untergeordnete Komponenten über die Änderungen informiert und verarbeitende Methoden aufgerufen. Während Änderungen am MainState und Timecode alle Rätsel-GameObjects informieren, wird bei Rätsel spezifischen States, wie Candles oder MazePosition, nur die entsprechende verarbeitende Funktion des einen Rätsels ausgeführt. Die Rätsel-GameObjects setzen den MainState bei wichtigen Fortschritt der Rätsel weiter.

### 3D-Modellierung

Für die 3D-Modelle haben wir einen eher simplen Stil gewählt. Die Modelle sind alle im Low-Poly Stil mit leicht abgerundeten Kanten modelliert, damit die Anzahl der Kanten im Spiel gering bleibt und es flüssig läuft. Zu dem Low-Poly Stil haben wir passend sehr einfache Texturen gewählt. Die unterschiedlichen Materialien auf jedem Objekt bestehen nur aus dem Principled BSDF Shader aus Blender oder im Falle der metallischen Materialien aus einem Unity eigenen Material, um die Reflexionen schnell und einfach zu ermöglichen.

Thematisch passen die Modelle zu den jeweiligen, von den einzelnen Rätseln repräsentierten Zeiten. Dies ermöglicht es dem Spieler schnell zu erkennen, welche Modelle zusammengehören. Die Modelle im Hintergrund unterliegen nicht diesen Bedingungen, da diese irrelevant für das Lösen der Rätsel sind und nur als Ablenkung für den Spieler dienen und den sonst leeren Hintergrund füllen sollen, damit dieser spannender wird.

Dementsprechend sind die relevantesten 3D-Modelle die, mit denen der Spieler für die einzelnen Rätsel interagieren muss. Dazu zählen der Maya Tempel mit seinen Stufen, die sich einklappen können und steinerne Knöpfe mit Symbolen freigeben, der Altar mit seinen Kerzen und dem Buch, um die Kiste zu öffnen, die Maschine mit dem Labyrinth, um den letzten Zeitkern zu befreien und die 3 Sockel in der Mitte, um die Zeitkerne hineinsetzen zu können.

### Physischer Controller

Zentrales Element der Koop-Spielerfahrung ist der eigens entwickelte und gebaute [physische Controller oder die Rätselbox](https://github.com/duevil/Between-Time-Controller). Auf Basis eines ESP32 werden aus einer Reihe kleinerer elektronischer Hardware-Komponenten Interface-Elemente erzeugt, welche mittels simpler Kommunikation über MQTT mit virtuellen Elementen innerhalb der Unity-Anwendung direkt interagieren können. Genauere Infos zum physischen Controller können in der README des entsprechenden Repositories gefunden werden, welche auch die eigens geschriebene Firmware des Controllers beinhaltet.

### Externe Ressourcen

Liste aller verwendeter externer Ressourcen/Assets:

- Quick Outline (Quick Outline | Particles/Effects | Unity Asset Store)
- Candle VFX - URP (Candle VFX - URP | Fire & Explosions | Unity Asset Store)
- Musik: Kevin McLeod - Special Harvest (Kevin MacLeod: Spacial Harvest, incompetech Music Search)
- Schrift: Triple Dot Digital-7 (Triple Dot Digital-7 Font | dafont.com)
- Lizensfreie Sounds von freesound.org
