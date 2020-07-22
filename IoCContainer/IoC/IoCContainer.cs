using System;
using System.Collections.Generic;
using System.Composition.Hosting;

namespace IoCContainer.IoC
{
    public class IoCContainer : IIoCContainer
    {
        private static IIoCContainer _instance;
        private readonly ITypeManager _typeManager;
        private static readonly object LockObject = new object();
        
        private CompositionHost Container { get; set; }
        private ContainerConfiguration Configuration { get; }

        private IoCContainer()
        {
            _typeManager = new TypeManager();
            Configuration = new ContainerConfiguration();
            InitializeContainer();
        }

        public static IIoCContainer Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (LockObject)
                {
                    _instance ??= new IoCContainer();
                }

                return _instance;
            }

            set => _instance = value;
        }

        public T Resolve<T>()
        {
            lock (LockObject)
            {
                try
                {
                    return Container.GetExport<T>();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public T Resolve<T>(string name)
        {
            lock (LockObject)
            {
                try
                {
                    return Container.GetExport<T>(name);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public IEnumerable<T> ResolveMany<T>()
        {
            lock (LockObject)
            {
                try
                {
                    return Container.GetExports<T>();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public IEnumerable<T> ResolveMany<T>(string name)
        {
            lock (LockObject)
            {
                try
                {
                    return Container.GetExports<T>(name);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        private void InitializeContainer()
        {
            if (Container != null)
                return;

            lock (LockObject)
            {
                if (Container != null)
                    return;

                Configuration.WithParts(_typeManager.GetExportedTypes());
                Container = Configuration.CreateContainer();
            }
        }
    }
}