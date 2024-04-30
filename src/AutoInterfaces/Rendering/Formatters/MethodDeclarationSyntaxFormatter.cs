using HandlebarsDotNet;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace AutoInterfaces.Rendering.Formatters
{
    internal class MethodDeclarationSyntaxFormatter : BaseFormatter<MethodDeclarationSyntax>
    {
        protected override void Format(MethodDeclarationSyntax target, in EncodedTextWriter writer)
        {
            
        }
    }
}
