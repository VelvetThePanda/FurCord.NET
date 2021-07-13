using System;
using Newtonsoft.Json;

namespace FurCord.NET.Entities.Converters
{
	internal sealed class ConcreteTypeConverter<TConcrete> : JsonConverter<TConcrete>
	{
		public override TConcrete? ReadJson(JsonReader reader, Type objectType, TConcrete? value, bool hasExistingValue, JsonSerializer serializer)
			=> serializer.Deserialize<TConcrete>(reader);

		public override void WriteJson(JsonWriter writer, TConcrete? value, JsonSerializer serializer)
			=> serializer.Serialize(writer, value);
	}
}