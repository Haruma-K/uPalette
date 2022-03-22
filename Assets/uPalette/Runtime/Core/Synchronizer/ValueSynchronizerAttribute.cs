using System;

namespace uPalette.Runtime.Core.Synchronizer
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class ValueSynchronizerAttribute : Attribute
    {
        protected ValueSynchronizerAttribute(Type valueType, Type attachTargetType, string targetPropertyDisplayName)
        {
            ValueType = valueType;
            AttachTargetType = attachTargetType;
            TargetPropertyDisplayName = targetPropertyDisplayName;
        }

        public Type ValueType { get; }

        public Type AttachTargetType { get; }

        public string TargetPropertyDisplayName { get; }
    }
}
