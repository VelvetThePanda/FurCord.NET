using System;
using Newtonsoft.Json;

namespace FurCord.NET.Entities.Converters
{
	public class ConcreteTypeConverter<TConcrete> : JsonConverter
	{
		public override bool CanConvert(Type objectType) => objectType.IsAssignableFrom(typeof(TConcrete));

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
			=> serializer.Deserialize<TConcrete>(reader);

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			=> serializer.Serialize(writer, value);
	}
}