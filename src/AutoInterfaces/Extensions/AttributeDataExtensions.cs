// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace AutoInterfaces.Extensions
{
    internal static class AttributeDataExtensions
    {
        /// <summary>
        /// Attempts to get an argument based on it's name
        /// </summary>
        public static bool TryGetArgument<T>(this AttributeData target, string argumentName, out T? result)
        {
            result = default;
            for(int i = 0; i < target.NamedArguments.Length; i++)
            {
                KeyValuePair<string, TypedConstant> argument = target.NamedArguments[i];

                if(string.Equals(argument.Key, argumentName, StringComparison.Ordinal))
                {
                    result = (T)argument.Value.Value!;
                    return true;
                }
            }
            return false;
        }
    }
}
