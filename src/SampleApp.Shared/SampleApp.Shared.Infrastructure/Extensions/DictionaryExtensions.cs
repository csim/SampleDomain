namespace SampleApp.Shared.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class DictionaryExtensions
    {
        public static IDictionary<string, T> Parse<T>(this IDictionary<string, T> source, string rawContent)
        {
            return rawContent
                .Split(";")
                .ToDictionary(
                    _ => _.Before("="),
                    _ =>
                    {
                        var val = _.After("=");
                        if (string.IsNullOrEmpty(val)) return default;

                        var type = typeof(T);
                        var coreType = Nullable.GetUnderlyingType(type);

                        return (T)Convert.ChangeType(val, coreType ?? type);
                    }
                );
        }
    }
}
