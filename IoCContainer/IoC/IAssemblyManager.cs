using System.Collections.Generic;
using System.Reflection;

namespace IoCContainer.IoC
{
    public interface IAssemblyManager
    {
        void AddAssembly<T>();
        IEnumerable<Assembly> GetAssemblies();
    }
}