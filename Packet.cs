using System;
using System.Text.Json.Serialization;

namespace TruckSimAT
{
    public class Instruction
    {
        [JsonPropertyName("type")]
        public string type { get; set; } = "";

        [JsonPropertyName("parameters")]
        [JsonConverter(typeof(ParametersConverter))]
        public object[] parameters { get; set; } = Array.Empty<object>();
    }

    public class Packet
    {
        [JsonPropertyName("instructions")]
        public Instruction[] instructions { get; set; } = Array.Empty<Instruction>();
    }
}
