using JsonSubTypes;
using Newtonsoft.Json;

namespace Library.Property
{
    [JsonConverter(typeof(JsonSubtypes), "Type")]
    [JsonSubtypes.KnownSubType(typeof(ArrayProperty), ArrayProperty.TypeName)]
    [JsonSubtypes.KnownSubType(typeof(BoolProperty), BoolProperty.TypeName)]
    [JsonSubtypes.KnownSubType(typeof(IntProperty), IntProperty.TypeName)]
    [JsonSubtypes.KnownSubType(typeof(NameProperty), NameProperty.TypeName)]
    [JsonSubtypes.KnownSubType(typeof(NoneProperty), NoneProperty.TypeName)]
    [JsonSubtypes.KnownSubType(typeof(StringProperty), StringProperty.TypeName)]
    [JsonSubtypes.KnownSubType(typeof(StructProperty), StructProperty.TypeName)]
    public interface IProperty
    {
        string Name { get; }
        string Type { get; }
    }
}
