using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TruckSimAT
{
    public class ParametersConverter : JsonConverter<object[]>
    {
        public override object[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException(); // Not needed for sending packets
        }

        public override void Write(Utf8JsonWriter writer, object[] value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var item in value)
            {
                if (item == null)
                {
                    writer.WriteNullValue();
                    continue;
                }

                switch (item)
                {
                    case int intValue:
                        writer.WriteNumberValue(intValue);
                        break;
                    case double doubleValue:
                        writer.WriteNumberValue(doubleValue);
                        break;
                    case string stringValue:
                        writer.WriteStringValue(stringValue);
                        break;
                    case bool boolValue:
                        writer.WriteBooleanValue(boolValue);
                        break;
                    case Enum enumValue:
                        writer.WriteNumberValue(Convert.ToInt32(enumValue));
                        break;
                    case List<int> listInt:
                        writer.WriteStartArray();
                        foreach (var num in listInt)
                        {
                            writer.WriteNumberValue(num);
                        }
                        writer.WriteEndArray();
                        break;
                    default:
                        writer.WriteStringValue(item.ToString());
                        break;
                }
            }
            writer.WriteEndArray();
        }
    }

    public static class Triggers
    {
        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            Converters =
            {
                new ParametersConverter()
            }
        };

        public static string PacketToJson(Packet packet)
        {
            return JsonSerializer.Serialize(packet, jsonOptions);
        }
    }
}
