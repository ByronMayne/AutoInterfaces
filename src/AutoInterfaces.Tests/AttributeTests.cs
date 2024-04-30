// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AutoInterfaces.Tests
{
    public class AttributeTests : TestBase
    {
        public AttributeTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public Task CustomNamespace()
            => ComposeAsync($$"""
                using AutoInterfaces;
                
                [AutoInterface(Namespace="CustomNamespace")]
                public class MyClass
                {
                    protected int PrivateValue { get; } 
                }
                """);
    }
}
