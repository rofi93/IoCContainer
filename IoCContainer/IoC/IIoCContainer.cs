using System.Collections.Generic;

namespace IoCContainer.IoC
{
    public interface IIoCContainer
    {
        T Resolve<T>();
        T Resolve<T>(string name);
        IEnumerable<T> ResolveMany<T>();
        IEnumerable<T> ResolveMany<T>(string name);
    }
}