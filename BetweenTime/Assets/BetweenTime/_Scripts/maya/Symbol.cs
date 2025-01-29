using UnityEngine;

namespace BetweenTime._Scripts.maya
{
    /// <summary>
    ///     Enum representing the possible mayan symbols,
    ///     with an extension method to get the corresponding sprite
    /// </summary>
    public enum Symbol
    {
        _0 = 19,
        _1 = 18,
        _2 = 17,
        _3 = 15,
        _4 = 13,
        _5 = 1,
        _6 = 12,
        _7 = 5,
        _8 = 10,
        _9 = 14,
        _10 = 0,
        _11 = 3
    }

    /// <summary>
    ///     Extension methods for the Symbol enum
    /// </summary>
    public static class SymbolExtension
    {
        // Array of all symbol sprites
        private static Sprite[] _sprites;

        /// <summary>
        ///     Gets the sprite corresponding to the symbol value
        /// </summary>
        /// <param name="symbol">
        ///     The symbol value to get the sprite for
        /// </param>
        /// <returns>
        ///     The sprite corresponding to the symbol value
        /// </returns>
        public static Sprite GetSprite(this Symbol symbol)
        {
            _sprites ??= Resources.LoadAll<Sprite>("symbols1");
            return _sprites[(int)symbol];
        }

        /// <summary>
        ///     Checks if the symbol is assignable to a button,
        ///     i.e. it is not one of the two extra symbols not used in the puzzle
        /// </summary>
        /// <param name="symbol">
        ///     The symbol to check
        /// </param>
        /// <returns>
        ///     True if the symbol is assignable, false otherwise
        /// </returns>
        public static bool Assignable(this Symbol symbol)
        {
            return symbol switch
            {
                Symbol._10 or Symbol._11 => false,
                _ => true
            };
        }
    }
}