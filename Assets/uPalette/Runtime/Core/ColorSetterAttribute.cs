using System;

namespace uPalette.Runtime.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ColorSetterAttribute : Attribute
    {
        public ColorSetterAttribute(Type targetType, string targetPropertyDisplayName)
        {
            TargetType = targetType;
            TargetPropertyDisplayName = targetPropertyDisplayName;
        }

        public Type TargetType { get; }

        public string TargetPropertyDisplayName { get; }
    }
}