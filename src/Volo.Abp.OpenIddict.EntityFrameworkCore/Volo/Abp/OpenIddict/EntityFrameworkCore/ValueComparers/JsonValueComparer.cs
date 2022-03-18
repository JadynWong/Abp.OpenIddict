using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Volo.Abp.Json.SystemTextJson.JsonConverters;

namespace Volo.Abp.OpenIddict.EntityFrameworkCore.ValueComparers;

public class JsonValueComparer<TPropertyType> : ValueComparer<TPropertyType>
{
    public JsonValueComparer()
        : base(
          (t1, t2) => DoEquals(t1, t2),
          t => DoGetHashCode(t),
          t => DoGetSnapshot(t))
    {
    }

    private static TPropertyType DoGetSnapshot(TPropertyType instance)
    {
        if (instance is ICloneable cloneable)
            return (TPropertyType)cloneable.Clone();

        return DeserializeObject(SerializeObject(instance));
    }

    private static int DoGetHashCode(TPropertyType instance)
    {
        if (instance is IEquatable<TPropertyType>)
            return instance.GetHashCode();

        return SerializeObject(instance).GetHashCode();
    }

    private static bool DoEquals(TPropertyType left, TPropertyType right)
    {
        if (left is IEquatable<TPropertyType> equatable)
            return equatable.Equals(right);

        var result = SerializeObject(left).Equals(SerializeObject(right));
        return result;
    }

    private static string SerializeObject(TPropertyType d)
    {
        return JsonSerializer.Serialize(d, JsonSerializerOptions);
    }

    private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions()
    {
        Converters =
        {
            new ObjectToInferredTypesConverter()
        },
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private static TPropertyType DeserializeObject(string s)
    {
        return JsonSerializer.Deserialize<TPropertyType>(s, JsonSerializerOptions);
    }
}
