using Identity.Application.DTOs.Attributes;
using System.Text.Json.Serialization;

namespace Identity.Application.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SupportedCulture
    {
        [EnumDescription("en")]
        en = 0,

        [EnumDescription("fa")]
        fa = 1,

        [EnumDescription("ar")]
        ar = 2
    }
}
