using System;

namespace StockExchange.Task.App.Helpers
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class CommandNameAttribute : Attribute
    {
        internal string Name { get; private set; }

        internal string Description { get; private set; }

        internal CommandNameAttribute(string name, string description = "")
        {
            Name = name;
            Description = description;
        }
    }
}
