namespace BetweenTime._Scripts.@base
{
    /// <summary>
    ///     Helper interface for parsing generic values to and from strings
    /// </summary>
    /// <typeparam name="T">The type of value to parse</typeparam>
    public interface IParser<T>
    {
        /// <summary>
        ///     Converts a string to a value of type T
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns>The value of type T</returns>
        T From(string value);

        /// <summary>
        ///     Converts a value of type T to a string
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The string representation of the value</returns>
        string To(T value)
        {
            return value.ToString();
        }
    }
}