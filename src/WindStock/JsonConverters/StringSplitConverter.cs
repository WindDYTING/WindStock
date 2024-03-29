using System;
using System.Linq;
using Newtonsoft.Json;

namespace WindStock.JsonConverters
{
    internal class StringSplitConverter<T> : JsonConverter<T[]>
    {
        private readonly string _separator;

        public StringSplitConverter(string separator)
        {
            _separator = separator;
        }

        public override void WriteJson(JsonWriter writer, T[] value, JsonSerializer serializer)
        {
            var json = string.Join(",", value);
            writer.WriteValue(json);
        }

        public override T[] ReadJson(JsonReader reader, Type objectType, T[] existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = reader.Value as string;
            var values = value.Trim('_').Split(_separator);
            T[] dest = values.Select(x => Convert.ChangeType(x, typeof(T))).Cast<T>().ToArray();
            return dest;
        }
    }
}
