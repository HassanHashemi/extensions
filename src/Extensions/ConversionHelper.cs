namespace Project.Base
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text;
    using System.Threading;

    /// <summary>Helper methods for converting values.</summary>
    public static class ConversionHelper
    {
        /// <summary>
        /// Enables conversion values for the given type from it's string representation.
        /// </summary>
        /// <typeparam name="T">The type to convert to. This can be a <see cref="Nullable"/> type.</typeparam>
        /// <param name="value">The string to convert.</param>
        /// <returns>An instance of the given <typeparamref name="T"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "Een ontwerp met 'object ConvertFromString(Type type, string value)' is " +
            "uiterst onhandig in deze situatie.")]
        [DebuggerStepThrough]
        public static T FromString<T>(string value)
        {
            return FromString<T>(value, Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// Enables conversion values for the given type from it's string representation.
        /// </summary>
        /// <typeparam name="T">The type to convert to. This can be a <see cref="Nullable"/> type.</typeparam>
        /// <param name="value">The string to convert.</param>
        /// <returns>An instance of the given <typeparamref name="T"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "Een ontwerp met 'object ConvertFromString(Type type, string value)' is " +
            "uiterst onhandig in deze situatie.")]
        [DebuggerStepThrough]
        public static T FromString<T>(string value, CultureInfo culture)
        {
            TypeConverter converter = GetConverterForType(typeof(T));

            try
            {
                return (T)converter.ConvertFromString(null, culture, value);
            }
            catch (Exception ex)
            {
                // HACK: Er zit een bug in het .NET framework (3.5sp1) in de BaseNumberConverter class. Deze
                // class gooit de Exception base class, daarom moeten wij 'Exception' catchen.
                throw new FormatException(
                    "The supplied value '" + value + "' could not be converted. " + ex.Message, ex);
            }
        }

        /// <summary>Converts an value to it's string representation.</summary>
        /// <remarks>Note that there is a corner case in which conversion is not correct in the case of using
        /// <see cref="Nullable{T}"/>. Converting an empty array and converting an array of one element with
        /// a value of null will convert to an empty string, while converting an empty string back will always
        /// result in an empty array.</remarks>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>The string representation of the value.</returns>
        [DebuggerStepThrough]
        public static string ToString<T>(T value)
        {
            return ObjectToString(value);
        }

        /// <summary> Converts an value to it's string representation.</summary>
        /// <param name="value">The value.</param>
        /// <returns>The string representation of the value.</returns>
        [DebuggerStepThrough]
        public static string ObjectToString(object value)
        {
            TypeConverter converter = GetConverterForType(value.GetType());

            return converter.ConvertToString(null, Thread.CurrentThread.CurrentCulture, value);
        }

        [DebuggerStepThrough]
        private static TypeConverter GetConverterForType(Type type)
        {
            if (type.IsArray)
            {
                Type arrayElementType = type.GetElementType();

                if (arrayElementType.IsValueType)
                {
                    // Voor ValueType arrays (zoals int[], Guid[] en int?[]) gebruiken we onze eigen converter
                    // omdat de .NET ArrayConverter comma separated strings niet kan converteren.
                    Type converterType =
                    typeof(ArrayConverter<>).MakeGenericType(new[] { arrayElementType });

                    return (TypeConverter)Activator.CreateInstance(converterType);
                }
            }

            return TypeDescriptor.GetConverter(type);
        }

        /// <summary>Converts arrays of values.</summary>
        /// <typeparam name="T">The type of the elements in the array.</typeparam>
        private sealed class ArrayConverter<T> : TypeConverter
        {
            private const char SplitChar = ',';

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string) ||
                this.CanConvertFrom(context, sourceType);
            }

            [DebuggerStepThrough]
            public override object ConvertTo(
                ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    if (value == null)
                    {
                        return null;
                    }

                    T[] collection = value as T[];

                    if (collection == null)
                    {
                        throw new ArgumentException("Invalid type " + value.GetType().FullName, "value");
                    }

                    return ConvertArrayToString(context, culture, collection);
                }

                return this.ConvertTo(context, culture, value, destinationType);
            }

            [DebuggerStepThrough]
            public override object ConvertFrom(
                ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value == null)
                {
                    return null;
                }

                string val = value as string;

                if (val != null)
                {
                    return ConvertStringToArray(context, culture, val);
                }

                return this.ConvertFrom(context, culture, value);
            }

            [DebuggerStepThrough]
            private static string ConvertArrayToString(
                ITypeDescriptorContext context, CultureInfo culture, T[] collection)
            {
                StringBuilder builder = new StringBuilder();

                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

                bool first = true;

                string splitChar = SplitChar.ToString();

                foreach (T t in collection)
                {
                    if (!first)
                    {
                        builder.Append(splitChar);
                    }

                    string convertedValue = converter.ConvertToString(context, culture, t);

                    builder.Append(convertedValue);

                    first = false;
                }

                return builder.ToString();
            }

            [DebuggerStepThrough]
            private static T[] ConvertStringToArray(
                ITypeDescriptorContext context, CultureInfo culture, string value)
            {
                if (value.Length == 0)
                {
                    // WARNING: The conversion is incorrect when converting an array of a nullable type
                    // with a single element with a value of null (i.e. new int?[1] { null }). This will
                    // convert to an empty string, while converting back would result in an array with no
                    // elements. The only way to solve this in this converter is to denote non-empty
                    // arrays with for instance brackets, but this would break existing code. We therefore
                    // move the responsibility of this to the caller.
                    return new T[0];
                }

                string[] elements = value.Split(new char[] { SplitChar });

                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

                T[] list = new T[elements.Length];
                int index = 0;

                foreach (string element in elements)
                {
                    try
                    {
                        list[index] = (T)converter.ConvertFromString(context, culture, element);
                        index++;
                    }
                    catch (FormatException ex)
                    {
                        // Throw a more expressive message.
                        throw new FormatException(ex.Message + " Supplied value: '" + element + "'.", ex);
                    }
                }

                return list;
            }
        }
    }
}
