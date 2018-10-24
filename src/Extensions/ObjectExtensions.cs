using System.Linq;
using System.Reflection;

namespace System
{
    public static class ObjectExtensions
    {
        public static bool HasProperty(this object source, string propertyName)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            return source.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Any(p => p.Name == propertyName);
        }

        public static object GetPropertyValue(this object source, string name)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            var type = source.GetType();
            var property = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p => p.Name == name);
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            return property.GetValue(source);
        }

        public static void SetProperty(this object source, string propertyName, object value)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            var type = source.GetType();
            var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                throw new InvalidOperationException($"property {propertyName} not found.");
            }
            property.SetValue(source, value);
        }
    }
}
