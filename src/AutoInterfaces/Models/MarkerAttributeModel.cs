namespace AutoInterfaces.Models
{
    internal class MarkerAttributeModel
    {
        /// <summary>
        /// Gets the name of the attribute class
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the namespace of the marker attribute 
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        /// Gets the name of the inheritance property 
        /// </summary>
        public string InheritancePropertyName { get; }

        /// <summary>
        /// Gets the name of the namespace property 
        /// </summary>
        public string NamespacePropertyName { get; }

        /// <summary>
        /// Gets the access modifier used by the class
        /// </summary>
        public ClassAccessModifier AccessModifier { get; set; }


        public MarkerAttributeModel()
        {
            Name = Constants.MarkerAttribute.ClassName;
            Namespace = Constants.MarkerAttribute.Namespace;
            InheritancePropertyName = Constants.MarkerAttribute.InheritancePropertyName;
            NamespacePropertyName = Constants.MarkerAttribute.NamespacePropertyName;
            AccessModifier = ClassAccessModifier.Internal;
        }
    }
}
