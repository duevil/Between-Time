using System;

namespace BetweenTime._Scripts.@base
{
    /// <summary>
    ///     Parser for converting basic types to and from strings
    /// </summary>
    /// <typeparam name="T">The type of value to parse; must implement <see cref="IConvertible" /></typeparam>
    public class BasicParser<T> : IParser<T> where T : IConvertible
    {
        public T From(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}