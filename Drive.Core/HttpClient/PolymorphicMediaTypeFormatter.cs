using System.Collections.Generic;
using System.Net.Http.Formatting;
using Newtonsoft.Json;

namespace Drive.Core.HttpClient
{
    /// <summary>
    /// The JSON media type formatter for deserialization of server responses in cases when DTO contains
    /// collections of abstract base types or interfaces.
    /// </summary>
    public static class PolymorphicMediaTypeFormatter
    {
        /// <summary>
        /// Gets the JSON media type formatter.
        /// </summary>
        /// <returns>The JSON media type formatter.</returns>
        public static JsonMediaTypeFormatter GetFormatter()
        {
            return new JsonMediaTypeFormatter { SerializerSettings = { TypeNameHandling = TypeNameHandling.Auto } };
        }

        /// <summary>
        /// Gets the enumerable with the only JSON media type formatter.
        /// </summary>
        /// <returns>The enumerable with the only JSON media type formatter.</returns>
        public static IEnumerable<JsonMediaTypeFormatter> GetFormatterAsEnumerable()
        {
            yield return GetFormatter();
        }
    }
}