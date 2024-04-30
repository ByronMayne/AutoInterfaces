using HandlebarsDotNet;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AutoInterfaces.Rendering.Formatters
{
    internal class PropertyDeclarationSyntaxFormatter : BaseFormatter<PropertyDeclarationSyntax>
    {
        private string? GetXmlDocs(CSharpSyntaxNode node)
        {
            SyntaxTriviaList leading = node.GetLeadingTrivia();
            for(int i = 0; i < leading.Count; i++)
            {
                SyntaxTrivia trivia = leading[i];

                switch (trivia.Kind())
                {
                    case SyntaxKind.SingleLineDocumentationCommentTrivia:
                    case SyntaxKind.MultiLineDocumentationCommentTrivia:
                        return trivia.ToString();
                    case SyntaxKind.WhitespaceTrivia:
                        continue;
                    default:
                        i = int.MaxValue;
                        break;
                }
            }

            return null;
        }

        protected override void Format(PropertyDeclarationSyntax target, in EncodedTextWriter writer)
        {
            ISet<SyntaxKind> accessors = GetAccessors(target.AccessorList);


            string? documenation = GetXmlDocs(target);

            if(!string.IsNullOrWhiteSpace(documenation))
            {
                writer.WriteSafeString($"/// {documenation}");
            }
            writer.Write("public ");
            writer.Write(target.Type);
            writer.Write(" ");
            writer.Write(target.Identifier.Text);
            writer.Write(" {");

            if (accessors.Contains(SyntaxKind.GetAccessorDeclaration))
            {
                writer.Write(" get; ");
            }

            if (accessors.Contains(SyntaxKind.SetAccessorDeclaration))
            {
                writer.Write("set; ");
            }
            else if (accessors.Contains(SyntaxKind.InitAccessorDeclaration))
            {
                writer.Write("init; ");
            }

            writer.Write("}");
        }

        private static ISet<SyntaxKind> GetAccessors(AccessorListSyntax? accessorList)
        {
            HashSet<SyntaxKind> result = [];

            if (accessorList != null)
            {
                foreach (AccessorDeclarationSyntax accessor in accessorList.Accessors)
                {
                    SyntaxKind syntaxKind = accessor.Kind();
                    switch (syntaxKind)
                    {
                        case SyntaxKind.GetAccessorDeclaration:
                        case SyntaxKind.SetAccessorDeclaration:
                        case SyntaxKind.InitAccessorDeclaration:
                            _ = result.Add(syntaxKind);
                            break;
                    }
                }
            }

            return result;
        }
    }
}
