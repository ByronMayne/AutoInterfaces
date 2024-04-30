using HandlebarsDotNet;
using HandlebarsDotNet.IO;
using System;

namespace AutoInterfaces.Rendering.Formatters
{
    /// <summary>
    /// Base render used for custom rendering of type in Handlebars
    /// </summary>
    /// <typeparam name="Target">The type it's used to render</typeparam>
    internal abstract class BaseFormatter<Target> : IFormatterProvider, IFormatter
    {
        /// <summary>
        /// Formats the given type to handlebrs
        /// </summary>
        protected abstract void Format(Target target, in EncodedTextWriter writer);

        /// <inheritdoc cref="IFormatter"/>
        void IFormatter.Format<T>(T value, in EncodedTextWriter writer)
            => Format((Target)(object)value, in writer);


        /// <inheritdoc cref="IFormatterProvider"/>
        bool IFormatterProvider.TryCreateFormatter(Type type, out IFormatter? formatter)
        {
            formatter = null;
            if (type == typeof(Target))
            {
                formatter = this;
                return true;
            }
            return false;
        }
    }
}
