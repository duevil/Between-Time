using System;

namespace BetweenTime._Scripts.@base
{
    /// <summary>
    ///     Possible states of the game's main state machine
    /// </summary>
    public enum MainState
    {
        Idle = 0,
        Started = 1,
        InputFieldOpened = 2,
        InputFieldSolved = 3,
        CandlesPlaced = 4,
        CandlesSolved = 5,
        BookBinarySolved = 6,
        MazeActive = 7,
        MazeSolved = 8,
        ArcadeUnlocked = 9,
        AllItemsScanned = 10,
        GameWon = 11,
        GameLost = 12
    }

    /// <summary>
    ///     Parser for converting <see cref="MainState" /> enum to and from strings
    /// </summary>
    public class MainStateParser : IParser<MainState>
    {
        public MainState From(string value)
        {
            return Enum.Parse<MainState>(value);
        }

        public string To(MainState value)
        {
            return value.ToString("D");
        }
    }
}