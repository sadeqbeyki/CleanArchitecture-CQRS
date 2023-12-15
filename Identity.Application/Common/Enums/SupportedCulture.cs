using Identity.Application.DTOs.Attributes;
using System.Text.Json.Serialization;

namespace Identity.Application.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SupportedCulture
    {
        [EnumDescription("en")]
        en = 0,

        [EnumDescription("hi")]
        hi = 1,

        [EnumDescription("fr")]
        fr = 2
    }
}
