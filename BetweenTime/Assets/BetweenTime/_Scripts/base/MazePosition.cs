namespace BetweenTime._Scripts.@base
{
    /// <summary>
    ///     Enum for representing the direction of a position in the maze
    /// </summary>
    public enum Direction
    {
        North,
        East,
        South,
        West,
        None
    }

    /// <summary>
    ///     Data structure for representing a position in the maze
    /// </summary>
    /// <param name="x">The horizontal position</param>
    /// <param name="y">The vertical position</param>
    /// <param name="direction">The direction in which the maze has moved after being updated</param>
    public record MazePosition(int x, int y, Direction direction)
    {
        /// <summary>
        ///     Default constructor; initializes the position to (0, 0)
        /// </summary>
        public MazePosition() : this(0, 0, Direction.None) { }

        /// <summary>
        ///     The horizontal position
        /// </summary>
        public int x { get; } = x;

        /// <summary>
        ///     The vertical position
        /// </summary>
        public int y { get; } = y;

        /// <summary>
        ///     The direction in which the maze has moved after being updated
        /// </summary>
        public Direction direction { get; } = direction;


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
                var direction = _lastValue switch
                {
                    { x: var lastX } when newX > lastX => Direction.East,
                    { x: var lastX } when newX < lastX => Direction.West,
                    { y: var lastY } when newY > lastY => Direction.North,
                    { y: var lastY } when newY < lastY => Direction.South,
                    _ => Direction.None
                };
                _lastValue = new MazePosition(newX, newY, direction);
                return _lastValue;
            }

            public string To(MazePosition value)
            {
                return ((value.x << 0) | (value.y << 4)).ToString();
            }
        }
    }
}