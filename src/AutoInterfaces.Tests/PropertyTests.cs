// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AutoInterfaces.Tests
{
    public class PropertyTests : TestBase
    {
        public PropertyTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public Task Public_GetSet_Included()
            => ComposeAsync("""
                using AutoInterfaces;
                
                [AutoInterface]
                public class MyClass
                {
                    public float PublicFloat { get; set; } 
                }
                """);

        [Fact]
        public Task Public_GetInit_Included()
            => ComposeAsync("""
                using AutoInterfaces;
                
                [AutoInterface]
                public class MyClass
                {
                    public float PublicFloat { get; init; } 
                }
                """);

        [Fact]
        public Task Public_Get_Included()
            => ComposeAsync("""
                using AutoInterfaces;
                
                [AutoInterface]
                public class MyClass
                {
                    public float PublicFloat { get; } 
                }
                """);

        [Fact]
        public Task Private_NotIncluded()
            => ComposeAsync("""
                using AutoInterfaces;
                
                [AutoInterface]
                public class MyClass
                {
                    private int PrivateValue { get; } 
                }
                """);

        [Fact]
        public Task Protected_NotIncluded()
            => ComposeAsync("""
                using AutoInterfaces;
                
                [AutoInterface]
                public class MyClass
                {
                    protected int PrivateValue { get; } 
                }
                """);

        [Fact]
        public Task Internal_NotIncluded()
            => ComposeAsync("""
                using AutoInterfaces;
                
                [AutoInterface]
                public class MyClass
                {
                    internal int PrivateValue { get; } 
                }
                """);
    }
}
