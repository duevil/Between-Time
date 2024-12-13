namespace BetweenTime._Scripts.@base
{
    /// <summary>
    ///     Data structure for representing the state of the candles
    /// </summary>
    /// <param name="value">The state of the candles as a bitfield</param>
    public record Candles(byte value)
    {
        /// <summary>
        ///     Default constructor; initializes the candles to all off
        /// </summary>
        public Candles() : this(0) { }

        /// Integer representing the state of the candles as a bitfield
        private byte value { get; } = value;

        /// <summary>
        ///     Gets the state of the candles at the specified index
        /// </summary>
        /// <param name="index">The index of the candle to check</param>
        public bool this[int index] => (value & (1 << index)) != 0;


        /// <summary>
        ///     Parser for converting a Candles object to and from a string
        /// </summary>
        public class Parser : BasicParser<byte>, IParser<Candles>
        {
            public new Candles From(string value)
            {
                return new Candles(base.From(value));
            }

            public string To(Candles value)
            {
                return value.value.ToString();
            }
        }
    }
}