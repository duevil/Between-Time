namespace BetweenTime._Scripts.@base
{
    /// <summary>
    ///     Data structure for representing a position in the maze
    /// </summary>
    /// <param name="x">The horizontal position</param>
    /// <param name="y">The vertical position</param>
    /// <param name="direction">The direction in which the maze has moved after being updated</param>
    public record MazePosition(int x, int y)
    {
        /// <summary>
        ///     Default constructor; initializes the position to (0, 0)
        /// </summary>
        public MazePosition() : this(0, 0) { }

        /// <summary>
        ///     The horizontal position
        /// </summary>
        public int x { get; } = x;

        /// <summary>
        ///     The vertical position
        /// </summary>
        public int y { get; } = y;


        /// <summary>
        ///     Parser for converting a MazePosition object to and from a string;
        ///     also calculates the direction in which the maze position has moved after being updated
        /// </summary>
        public class Parser : BasicParser<byte>, IParser<MazePosition>
        {
            private MazePosition _lastValue;

            public new MazePosition From(string value)
            {
                var intValue = base.From(value);
                var newX = (intValue >> 0) & 0xF;
                var newY = (intValue >> 4) & 0xF;
                _lastValue = new MazePosition(newX, newY);
                return _lastValue;
            }

            public string To(MazePosition value)
            {
                return ((value.x << 0) | (value.y << 4)).ToString();
            }
        }
    }
}