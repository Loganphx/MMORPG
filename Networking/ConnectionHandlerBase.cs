using System;
using System.Reflection;

namespace Networking
{
    public abstract class ConnectionHandlerBase<T>
    {
        protected abstract void HandleUnknownPackage(T connection, object parsedData, CommuncationPackage type);

        public void InvokeAction(T connection, object parsedData, CommuncationPackage type)
        {
            foreach (var method in GetType().GetMethods())
            {
                var attribute = method.GetCustomAttribute<PackageHandlerAttribute>();
            
                if(attribute == null) continue;

                if (attribute.Type == type)
                {
                    method.Invoke(this, new object?[] { connection, parsedData });
                    return;
                }
            }

            HandleUnknownPackage(connection, parsedData, type);
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class PackageHandlerAttribute : Attribute
    {
        public readonly CommuncationPackage Type;

        public PackageHandlerAttribute(CommuncationPackage type)
        {
            Type = type;
        }
    
    }
}
