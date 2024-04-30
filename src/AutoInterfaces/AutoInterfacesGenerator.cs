// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using AutoInterfaces.Models;
using AutoInterfaces.Rendering;
using HandlebarsDotNet;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using SGF;

namespace AutoInterfaces
{

    [SgfGenerator]
    public class AutoInterfacesGenerator : IncrementalGenerator
    {
        public AutoInterfacesGenerator() : base("AutoInterfaces")
        {
        }

        public override void OnInitialize(SgfInitializationContext context)
        {
            // Generate Marker Attribute 
            context.RegisterPostInitializationOutput(static (context) =>
            {
                var model = new MarkerAttributeModel();
                var classDefinition = Renderer.Render("MarkerAttribute", model);
                context.AddSource($"{model.Namespace}_{model.Name}.g.cs", classDefinition);
            });

            var interfaceProvider = context.SyntaxProvider
                .ForAttributeWithMetadataName(
                    Constants.MarkerAttribute.FullyQualifiedName,
                    predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                    transform: static (c, _) => GetSemanticTargetForGeneration(c))
                .Where(static m => m is not null);
            context.RegisterSourceOutput(interfaceProvider, GenerateInterface!);

        }

        private void GenerateInterface(SgfSourceProductionContext context, InterfaceModel model)
        {
            SourceText classDefinition = Renderer.Render("Interface", model);
            context.AddSource($"{model.Namespace}_{model.Name}.g.cs", classDefinition);
        }

        /// <summary>
        /// Return back if the node is valid for our source generator or not.
        /// </summary>
        /// <param name="node">The node to check</param>
        /// <returns>True if it's valid otherwise false</returns>
        private static bool IsSyntaxTargetForGeneration(SyntaxNode node)
            => node is ClassDeclarationSyntax;

        private static InterfaceModel? GetSemanticTargetForGeneration(GeneratorAttributeSyntaxContext context)
        {
            ClassDeclarationSyntax classDeclaration = (ClassDeclarationSyntax)context.TargetNode;
            SemanticModel semanticModel = context.SemanticModel;
            AttributeData? attribute = context.Attributes.FirstOrDefault();

            return attribute is null
                ? null
                : InterfaceModel.Generate(classDeclaration, attribute, semanticModel);
        }
    }
}
