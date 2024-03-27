using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AlgorithmDLX.UnitTest.Classes.Converters
{
    internal class SingleLineArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            // Include bool[][] in the types this converter can handle
            return objectType == typeof(int[][])
                || objectType == typeof(List<List<int>>)
                || objectType == typeof(bool[][]);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                throw new JsonSerializationException("Value is null.");
            }

            // Handle int[][] serialization
            if (value is int[][] intArray)
            {
                WriteArray(writer, intArray);
            }
            // Handle List<List<int>> serialization
            else if (value is List<List<int>> intList)
            {
                WriteList(writer, intList);
            }
            // Handle bool[][] serialization
            else if (value is bool[][] boolArray)
            {
                WriteArray(writer, boolArray);
            }
            else
            {
                throw new JsonSerializationException("Unsupported type for serialization.");
            }
        }

        private static void WriteArray<T>(JsonWriter writer, T[][] array)
        {
            writer.WriteStartArray(); // Start the outer array

            foreach (var subArray in array)
            {
                JArray jArray = new JArray(subArray);
                writer.WriteRawValue(jArray.ToString(Formatting.None)); // Write each sub-array
            }

            writer.WriteEndArray(); // End the outer array
        }

        private static void WriteList<T>(JsonWriter writer, List<List<T>> list)
        {
            writer.WriteStartArray(); // Start the outer array

            foreach (var subList in list)
            {
                JArray jArray = new JArray(subList);
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
