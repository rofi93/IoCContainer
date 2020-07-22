using System;
using System.Reflection;
using System.Collections.Generic;

namespace IoCContainer.IoC
{
    public interface ITypeManager
    {
        void AddType<T>();
        void AddExportedTypes(Assembly assembly);
        IEnumerable<Type> GetExportedTypes();
    }
}