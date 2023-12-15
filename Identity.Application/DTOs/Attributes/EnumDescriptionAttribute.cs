namespace Identity.Application.DTOs.Attributes
{
    public interface IAttribute<T>
    {
        T Value { get; }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumDescriptionAttribute : Attribute, IAttribute<string>
    {
        public EnumDescriptionAttribute(string value)
        {
            this.Value = value;
        }

        public string Value { get; }
    }

}
