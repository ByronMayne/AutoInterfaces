using AutoInterfaces.Rendering.Formatters;
using HandlebarsDotNet;
using HandlebarsDotNet.IO;
using Microsoft.CodeAnalysis.Text;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace AutoInterfaces.Rendering
{

    /// <summary>
    /// Used to render handlebar templates into <see cref="SourceText"/>
    /// </summary>
    internal class Renderer
    {
        private static readonly Encoding s_encoding;
        private static Assembly s_assembly;
        private static IHandlebars s_handlebars;

        static Renderer()
        {
            Type type = typeof(Renderer);
            s_assembly = type.Assembly;
            s_encoding = Encoding.UTF8;

            HandlebarsConfiguration configuration = new HandlebarsConfiguration();
            configuration.FormatterProviders.Add(new EventDeclarationSyntaxFormatter());
            configuration.FormatterProviders.Add(new MethodDeclarationSyntaxFormatter());
            configuration.FormatterProviders.Add(new PropertyDeclarationSyntaxFormatter());
            configuration.FormatterProviders.Add(new TypeSyntaxFormatter());
            s_handlebars = Handlebars.Create(configuration);
            s_handlebars.RegisterTemplate("property", """
                // Comment 
                public int FloatN { get; };
                """);
        }

        public static SourceText Render<T>(string templateName, T context) where T : class
        {
            HandlebarsTemplate<object, object> renderer;

            using (Stream stream = s_assembly.GetManifestResourceStream(templateName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string content = reader.ReadToEnd();
                renderer = s_handlebars.Compile(content);
            }
            string classDefiniation = renderer(context);
            return SourceText.From(classDefiniation, s_encoding);
        }
    }
}
