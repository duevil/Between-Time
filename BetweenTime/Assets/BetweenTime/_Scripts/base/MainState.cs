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
        CandlesSolved = 4,
        BookBinarySolved = 5,
        MazeActive = 6,
        MazeSolved = 7,
        ArcadeUnlocked = 8,
        AllItemsScanned = 9,
        GameWon = 10,
        GameLost = 11
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