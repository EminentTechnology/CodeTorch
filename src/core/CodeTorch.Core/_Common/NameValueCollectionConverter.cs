using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CodeTorch.Core
{
    public class NameValueCollectionConverter : JsonConverter<NameValueCollection>
    {
        private readonly HashSet<string> _excludedParameters;

        public NameValueCollectionConverter(IEnumerable<string> excludedParameters)
        {
            _excludedParameters = new HashSet<string>(excludedParameters, StringComparer.OrdinalIgnoreCase);
        }

        public override void WriteJson(JsonWriter writer, NameValueCollection value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            foreach (var key in value.AllKeys)
            {
                if (!_excludedParameters.Contains(key))
                {
                    writer.WritePropertyName(key);
                    writer.WriteValue(value[key]);
                }
                else
                {
                    writer.WritePropertyName(key);
                    writer.WriteValue("********");
                }
            }
            writer.WriteEndObject();
        }

        public override NameValueCollection ReadJson(JsonReader reader, Type objectType, NameValueCollection existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
