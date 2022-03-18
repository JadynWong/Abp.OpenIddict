using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volo.Abp.Json.SystemTextJson.JsonConverters;

namespace Volo.Abp.OpenIddict.EntityFrameworkCore.ValueConverters;

public class JsonValueConverter<TPropertyType> : ValueConverter<TPropertyType, string>
{
    public JsonValueConverter()
        : base(
            d => SerializeObject(d),
            s => DeserializeObject(s))
    {

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
