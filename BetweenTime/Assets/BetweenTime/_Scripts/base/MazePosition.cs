namespace BetweenTime._Scripts.@base
{
    /// <summary>
    ///     Data structure for representing a position in the maze
    /// </summary>
    /// <param name="X">The horizontal position</param>
    /// <param name="Y">The vertical position</param>
    public record MazePosition(int X, int Y)
    {
        /// <summary>
        ///     Default constructor; initializes the position to (0, 0)
        /// </summary>
        public MazePosition() : this(0, 0) { }

        /// <summary>
        ///     The horizontal position
        /// </summary>
        public int X { get; } = X;

        /// <summary>
        ///     The vertical position
        /// </summary>
        public int Y { get; } = Y;


        /// <summary>
        ///     Parser for converting a MazePosition object to and from a string
        /// </summary>
        public class Parser : BasicParser<byte>, IParser<MazePosition>
        {
            public new MazePosition From(string value)
            {
                var intValue = base.From(value);
                return new MazePosition((intValue >> 0) & 0xF, (intValue >> 4) & 0xF);
            }

            public string To(MazePosition value)
            {
                return ((value.X << 0) | (value.Y << 4)).ToString();
            }
        }
    }
}