using AutoInterfaces.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace AutoInterfaces.Models
{
    internal class InterfaceModel
    {
        /// <summary>
        /// Gets the interface name 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the namespace for the interface
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        /// Gets the list of interfaces this interface should inherit from.
        /// </summary>
        public IList<string> Inheritance { get; set; }

        /// <summary>
        /// Gets the access modifier for the interface
        /// </summary>
        public ClassAccessModifier AccessModifier { get; }

        /// <summary>
        /// Gets the list of properties that are defined in the class and public 
        /// </summary>
        public IList<PropertyDeclarationSyntax> Properties { get; set; }

        /// <summary>
        /// Gets the list of methods defined in the class which are public 
        /// </summary>
        public IList<MethodDeclarationSyntax> Methods { get; set; }

        /// <summary>
        /// Gets the list of events defined in the class which are public 
        /// </summary>
        public IList<EventDeclarationSyntax> Events { get; set; }

        /// <summary>
        /// Gets a list of all the indexers in the interface
        /// </summary>
        public IList<IndexerDeclarationSyntax> Indexers { get; private set; }

        public InterfaceModel(string name, string @namespace, ClassAccessModifier accessModifier)
        {
            Name = name;
            Namespace = @namespace;
            AccessModifier = accessModifier;
            Inheritance = Array.Empty<string>();
            Indexers = Array.Empty<IndexerDeclarationSyntax>(); 
            Properties = Array.Empty<PropertyDeclarationSyntax>();
            Methods = Array.Empty<MethodDeclarationSyntax>();
            Events = Array.Empty<EventDeclarationSyntax>();
        }

        /// <summary>
        /// Generates an <see cref="InterfaceModel"/> from the given <see cref="ClassDeclarationSyntax"/>.
        /// </summary>
        /// <param name="classDeclaration">The class syntax</param>
        /// <param name="attributeData">The attribute syntax</param>
        /// <returns></returns>
        internal static InterfaceModel? Generate(ClassDeclarationSyntax classDeclaration, AttributeData attributeData, SemanticModel semanticModel)
        {
            INamedTypeSymbol? typeSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);
            if (typeSymbol == null)
            {
                return null;
            }

            if (!attributeData.TryGetArgument(Constants.MarkerAttribute.InheritancePropertyName, out string? @namespace))
            {
                @namespace = typeSymbol.ContainingNamespace.Name;
            }

            string className = typeSymbol.Name;
            ClassAccessModifier accessModifier = classDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword))
                ? ClassAccessModifier.Public
                : ClassAccessModifier.Internal;

            var isExplictImplemention = CreateExplictInterfacePredicate($"I{className}");
            var isInterfaceMember = CreateIsInterfaceMemberPredicate();

            List<PropertyDeclarationSyntax> properties = new List<PropertyDeclarationSyntax>();
            List<MethodDeclarationSyntax> methods = new List<MethodDeclarationSyntax>();
            List<EventDeclarationSyntax> events = new List<EventDeclarationSyntax>();
            List<IndexerDeclarationSyntax> indexers = new List<IndexerDeclarationSyntax>();

            foreach (MemberDeclarationSyntax memberDeclaration in classDeclaration.Members)
            {
                switch (memberDeclaration)
                {
                    case PropertyDeclarationSyntax propertyDeclaration:
                        if (isInterfaceMember(propertyDeclaration) ||
                            isExplictImplemention(propertyDeclaration.ExplicitInterfaceSpecifier))
                        {
                            properties.Add(propertyDeclaration);
                        }
                        break;
                    case MethodDeclarationSyntax methodDeclaration:
                        if (isInterfaceMember(methodDeclaration) ||
                            isExplictImplemention(methodDeclaration.ExplicitInterfaceSpecifier))
                        {
                            methods.Add(methodDeclaration);
                        }
                        break;
                    case EventDeclarationSyntax eventDeclaration:
                        if (isInterfaceMember(eventDeclaration) ||
                            isExplictImplemention(eventDeclaration.ExplicitInterfaceSpecifier))
                        {
                            events.Add(eventDeclaration);
                        }
                        break;
                    case IndexerDeclarationSyntax indexerDeclaration:
                        if (isInterfaceMember(indexerDeclaration) ||
                            isExplictImplemention(indexerDeclaration.ExplicitInterfaceSpecifier))
                        {
                            indexers.Add(indexerDeclaration);
                        }
                        break;
                }
            }



            return new InterfaceModel(className, @namespace, accessModifier)
            {
                Methods = methods,
                Events = events,
                Properties = properties,
                Indexers = indexers
            };
        }

        /// <summary>
        /// Creates a predeicate for checking if the member should be exposed to an interface.
        /// </summary>
        /// <returns>The predicate to use</returns>
        static Predicate<MemberDeclarationSyntax> CreateIsInterfaceMemberPredicate()
            => member =>
            {
                bool isPublic = false;
                bool isStatic = false;

                foreach (SyntaxToken modifier in member.Modifiers)
                {
                    if (modifier.IsKind(SyntaxKind.PublicKeyword))
                    {
                        isPublic = true;
                    }

                    if (modifier.IsKind(SyntaxKind.StaticKeyword))
                    {
                        isStatic = true;
                    }
                }
                return isPublic && !isStatic;
            };

        /// <summary>
        /// Creates a predicate that is used to check if the member is implemented using an explict interface
        /// specifier that matches the current interface name.
        /// </summary>
        /// <param name="interfaceName">The name of the interface</param>
        /// <returns>The predicate to use</returns>
        static Predicate<ExplicitInterfaceSpecifierSyntax?> CreateExplictInterfacePredicate(string interfaceName)
            => (explictInterface) =>
            {
                if (explictInterface == null)
                {
                    return false;
                }
                string actualName = explictInterface.Name.ToString();
                return string.Equals(actualName, interfaceName);
            };
    }
}
