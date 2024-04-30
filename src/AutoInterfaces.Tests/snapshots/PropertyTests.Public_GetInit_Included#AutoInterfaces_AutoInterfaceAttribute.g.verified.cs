//HintName: AutoInterfaces_AutoInterfaceAttribute.g.cs
#nullable enable
using System;

namespace AutoInterfaces
{
    /// <summary> 
    /// When applied to a class this will mark it for the source generator to generate 
    /// an interface for it automatically. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal sealed class AutoInterfaceAttribute : Attribute 
    {
        /// <summary>
        /// Gets the list of interfaces that this interface should inheirt from.
        /// </summary>
        public Type[] Inheritance? { get; set; }

        /// <summary>
        /// Gets or sets the namespace used by the interface, when not defined it used the current namespace.
        /// </summary>
        public string Namespace? { get; set; }
        
        public AutoInterfaceAttribute()
        {   
            Inheritance = null;
            Namespace = null;
        }
    }
}