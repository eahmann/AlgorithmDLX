using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace AlgorithmDLX.UnitTest.Classes.Converters
{
    internal class SingleLineArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int[][]) || objectType == typeof(List<List<int>>);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {

            if (value == null)
            {
                throw new JsonSerializationException("Value is null.");
            }
            
            if (value is int[][] array)
            {
                WriteIntArray(writer, array);
            }
            else if (value is List<List<int>> list)
            {
                WriteIntList(writer, list);
            }
            else
            {
                throw new JsonSerializationException("Expected int[][] or List<List<int>>");
            }
        }

        private static void WriteIntArray(JsonWriter writer, int[][] array)
        {
            writer.WriteStartArray(); // Start the outer array

            foreach (var subArray in array)
            {
                JArray jArray = new(subArray);
                writer.WriteRawValue(jArray.ToString(Formatting.None)); // Write each sub-array
            }

            writer.WriteEndArray(); // End the outer array
        }

        private static void WriteIntList(JsonWriter writer, List<List<int>> list)
        {
            writer.WriteStartArray(); // Start the outer array

            foreach (var subList in list)
            {
                JArray jArray = new(subList);
                writer.WriteRawValue(jArray.ToString(Formatting.None)); // Write each sub-list
            }

            writer.WriteEndArray(); // End the outer array
        }


        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanRead => false;
    }
}
