using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JsonTest2.Model
{
    class Converters
    {
    }

    public class LegalPersonConverterWithTypeDiscriminator : JsonConverter<IPerson>
    {
        enum TypeDiscriminator
        {
            LegalPerson = 1,
            NaturalPerson = 2
        }

        public override bool CanConvert(Type typeToConvert) =>
            typeof(IPerson).IsAssignableFrom(typeToConvert);

        public override IPerson Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            string propertyName = reader.GetString();
            if (propertyName != "TypeDiscriminator")
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }

            TypeDiscriminator typeDiscriminator = (TypeDiscriminator)reader.GetInt32();
            IPerson person = typeDiscriminator switch
            {
                TypeDiscriminator.LegalPerson => new LegalPerson(),
                TypeDiscriminator.NaturalPerson => new NaturalPerson(),
                _ => throw new JsonException()
            };

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return person;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "NameLegal":
                            string nameLegal = reader.GetString();
                            ((LegalPerson)person).NameLegal = nameLegal;
                            break;
                        case "NaturalPerson":
                            string naturalname = reader.GetString();
                            ((NaturalPerson)person).NaturalName = naturalname;
                            break;
                        case "Id":
                            int id = reader.GetInt32();
                            person.Id = id;
                            break;
                    }
                }
            }

            throw new JsonException();
        }

        public override void Write(
            Utf8JsonWriter writer, IPerson person, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (person is LegalPerson legalPerson)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.LegalPerson);
                writer.WriteString("NameLegal", legalPerson.NameLegal);
            }
            else if (person is NaturalPerson naturalPerson)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.NaturalPerson);
                writer.WriteString("NaturalName", naturalPerson.NaturalName);
            }

            writer.WriteNumber("Id", person.Id);

            writer.WriteEndObject();
        }
    }

    public class PersonTypeConverterV2 : JsonConverter<IPerson>
    {
        public override IPerson Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, IPerson value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

    public class PersonTypeConverter : JsonConverter<IPerson>
    {
        // Any child type of ApiFieldType can be deserialized
        public override bool CanConvert(Type objectType) => typeof(IPerson).IsAssignableFrom(objectType);

        // We'll get to this one in a bit...
        public override IPerson Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Check for null values
            if (reader.TokenType == JsonTokenType.Null) return null;

            // Copy the current state from reader (it's a struct)
            var readerAtStart = reader;

            // Read the `className` from our JSON document
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            var jsonObject = jsonDocument.RootElement;

            var className =  jsonObject.GetProperty("ClassName").GetString();
           // Console.WriteLine("className: " + className);

            // See if that class can be deserialized or not
            if (!string.IsNullOrEmpty(className) && TypeMap.TryGetValue(className, out var targetType))
            {
                // Deserialize it
                return JsonSerializer.Deserialize(ref readerAtStart, targetType, options) as IPerson;
            }

            throw new NotSupportedException($"{className ?? "<unknown>"} can not be deserialized");
        }

        public override void Write(Utf8JsonWriter writer, IPerson value, JsonSerializerOptions options)
        {
            // No need for this one in our use case, but to just dump the object into JSON
            // (without having the className property!), we can do this:
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }

        private static readonly Dictionary<string, Type> TypeMap = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
        {
            { "PersonType.LegalPerson", typeof(LegalPerson) },
            { "PersonType.NaturalPerson", typeof(NaturalPerson)}
        };
    }

  


}
