using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StockExchange.Task.App.Commands;

namespace StockExchange.Task.App.Helpers
{
    internal static class CommandHelper
    {
        internal static bool IsCommand(Type type)
        {
            return typeof(ICommand).IsAssignableFrom(type) && Attribute.GetCustomAttribute(type, typeof(CommandNameAttribute)) != null;
        }

        internal static CommandNameAttribute GetCommandName(Type type)
        {
            return (CommandNameAttribute)Attribute.GetCustomAttribute(type, typeof(CommandNameAttribute));
        }

        internal static IEnumerable<CommandNameAttribute> GetAvailableCommandNames(Assembly assembly)
        {
            return assembly.GetTypes().Where(IsCommand).Select(GetCommandName);
        }
    }
}
