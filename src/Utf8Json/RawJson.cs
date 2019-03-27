using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Utf8Json
{
    public struct RawJson
    {
        public ArraySegment<byte> Data;
        internal IJsonFormatterResolver _resolver;

        public T Deserialize<T>()
        {
            return JsonSerializer.Deserialize<T>(Data.ToArray(), _resolver);
        }

        public static implicit operator RawJson(ArraySegment<byte> data)
        {
            return new RawJson { Data = data };
        }
        public static implicit operator byte[](RawJson data)
        {
            return data.Data.ToArray();
        }
    }
}
