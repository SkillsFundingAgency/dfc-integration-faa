using System;

namespace DFC.Integration.AVFeed.Repository.Sitefinity.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class JsonIgnoreSerializationAttribute : Attribute { }
}
