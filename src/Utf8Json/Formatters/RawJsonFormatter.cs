using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utf8Json.Formatters
{
    class RawJsonFormatter : IJsonFormatter<RawJson>
    {
        public static IJsonFormatter<RawJson> Default = new RawJsonFormatter();

        public RawJson Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
        {
            var token = reader.GetCurrentJsonToken();

            var rawJson = new RawJson { };
            rawJson._resolver = formatterResolver;

            switch (token)
            {
                case JsonToken.Null:
                {
                    return rawJson;
                }

                case JsonToken.BeginArray:
                case JsonToken.BeginObject:
                {
                    rawJson.Data = reader.ReadNextBlockSegment();
                    return rawJson;
                }
                case JsonToken.String:
                {
                    rawJson.Data = reader.ReadStringSegmentRaw();
                    return rawJson;
                }
                case JsonToken.Number:
                {
                    rawJson.Data = reader.ReadNumberSegment();
                    return rawJson;
                }
                case JsonToken.True:
                case JsonToken.False:
                {
                    int offset = reader.GetCurrentOffsetUnsafe();
                    reader.ReadBoolean();
                    rawJson.Data = new ArraySegment<byte>(
                        reader.GetBufferUnsafe(),
                        offset,
                        reader.GetCurrentOffsetUnsafe() - offset);
                    return rawJson;
                }
            }
            throw new NotSupportedException();
        }

        public void Serialize(ref JsonWriter writer, RawJson value, IJsonFormatterResolver formatterResolver)
        {
            if(value.Data.Count == 0)
            {
                writer.WriteNull();
            }
            writer.WriteRaw(value.Data.ToArray());
        }
    }
}
