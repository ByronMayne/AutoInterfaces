using System;
using HandlebarsDotNet;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoInterfaces.Rendering.Formatters
{
    internal class TypeSyntaxFormatter : BaseFormatter<TypeSyntax>
    {
        protected override void Format(TypeSyntax target, in EncodedTextWriter writer)
        {
            string typeName = GetTypeSytaxName(target);
            writer.Write(typeName);
        }


        private static string GetTypeSytaxName(TypeSyntax typeSyntax)
        {
            switch (typeSyntax.Kind())
            {
                case SyntaxKind.NullableType:
                    NullableTypeSyntax nullableTypeSyntax = (NullableTypeSyntax)typeSyntax;
                    return $"{GetTypeSytaxName(nullableTypeSyntax.ElementType)}?";
                case SyntaxKind.StringKeyword: return "string";
                case SyntaxKind.IntKeyword: return "int";
                case SyntaxKind.ShortKeyword: return "short";
                case SyntaxKind.LongKeyword: return "long";
                case SyntaxKind.FloatKeyword: return "float";
                case SyntaxKind.ULongKeyword: return "ulong";
                case SyntaxKind.UShortKeyword: return "ushort";
                case SyntaxKind.SByteKeyword: return "sbyte";
                case SyntaxKind.DoubleKeyword: return "double";
                case SyntaxKind.DecimalKeyword: return "decimal";
                case SyntaxKind.IdentifierName:
                    IdentifierNameSyntax identifierNameSyntax = (IdentifierNameSyntax)typeSyntax;
                    return identifierNameSyntax.Identifier.Text;

                default:
                    return "object";
            }
        }
    }
}
