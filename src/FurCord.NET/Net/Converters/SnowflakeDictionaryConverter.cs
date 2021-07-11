using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FurCord.NET.Entities
{
	internal class SnowflakeDictionaryConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
		{
			if (value is null)
				writer.WriteNull();
			else
				JToken.FromObject(value.GetType().GetTypeInfo().GetDeclaredProperty("Values")!.GetValue(value)!).WriteTo(writer);
		}

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
		{
			ConstructorInfo ctor = objectType
				.GetTypeInfo()
				.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null,
					CallingConventions.Any, Array.Empty<Type>(), null)!;

			object dict = ctor.Invoke(Array.Empty<object>());

			PropertyInfo indexer = objectType.GetTypeInfo().GetDeclaredProperty("Item")!;
			var entries = (IEnumerable) serializer.Deserialize(reader, objectType.GenericTypeArguments[1].MakeArrayType())!;

			foreach (var ent in entries)
				indexer.SetValue(dict, ent, new object[] {(ent as ISnowflake)!.Id});


			return null;
		}

		public override bool CanConvert(Type objectType)
		{
			var genericTypedef = objectType.GetGenericTypeDefinition();

			if (genericTypedef.IsAssignableTo(typeof(IDictionary<,>)))
				return objectType.GenericTypeArguments[0] == typeof(ulong) &&
				       genericTypedef.GenericTypeArguments[1].IsAssignableTo(typeof(ISnowflake));

			if (!genericTypedef.IsAssignableTo(typeof(ConcurrentDictionary<,>))) return false;

			return objectType.GenericTypeArguments[0] == typeof(ulong) &&
			       genericTypedef.GenericTypeArguments[1].IsAssignableTo(typeof(ISnowflake));
		}
	}
}