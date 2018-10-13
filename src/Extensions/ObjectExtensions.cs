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
                return false;
            }
            return source.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Any(p => p.Name == propertyName);
        }

        public static object GetPropertyVaue(this object source, string name)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            var type = source.GetType();
            var property = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p => p.Name == name);

            return property.GetValue(source);
        }

        public static void SetProperty(this object source, string propertyName, object value)
        {
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
