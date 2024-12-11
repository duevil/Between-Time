using UnityEngine;

namespace BetweenTime._Scripts.maya
{
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

    public static class SymbolExtension
    {
        private static Sprite[] _sprites;

        public static Sprite GetSprite(this Symbol symbol)
        {
            _sprites ??= Resources.LoadAll<Sprite>("symbols1");
            return _sprites[(int)symbol];
        }

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