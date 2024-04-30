// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using VerifyXunit;
using Xunit.Abstractions;

namespace AutoInterfaces.Tests
{
    public abstract class TestBase
    {
        private readonly ITestOutputHelper m_outputHelper;

        public TestBase(ITestOutputHelper testOutputHelper)
        {
            m_outputHelper = testOutputHelper;
        }

        protected Task ComposeAsync(string source)
        {
            // Parse the provided string into a C# syntax tree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);

            // Create a Roslyn compilation for the syntax tree.
            CSharpCompilation compilation = CSharpCompilation.Create(
                    assemblyName: "Tests",
                    syntaxTrees: new[] { syntaxTree },
                    references: new[] {
                        MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location)
                    }
                ); ;

            // Create an instance of our hoist used to wrap the source generator
            AutoInterfacesGeneratorHoist hoist = new AutoInterfacesGeneratorHoist();

            // The GeneratorDriver is used to run our generator against a compilation
            GeneratorDriver driver = CSharpGeneratorDriver.Create(hoist);

            // Run the source generator!
            driver = driver.RunGenerators(compilation);

#if DEBUG
            GeneratorDriverRunResult runResults = driver.GetRunResult();
            foreach (SyntaxTree generatedTree in runResults.GeneratedTrees)
            {
                string filename = Path.GetFileNameWithoutExtension(generatedTree.FilePath);
                m_outputHelper.WriteLine($"------------------{filename}-----------------------");
                m_outputHelper.WriteLine(generatedTree.ToString());
            }
#endif

            // Use verify to snapshot test the source generator output!
            return Verifier.Verify(driver)
                .UseDirectory("snapshots");
        }
    }
}
