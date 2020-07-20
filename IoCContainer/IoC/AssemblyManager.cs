using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace IoCContainer.IoC
{
    public class AssemblyManager : IAssemblyManager
    {
        private readonly Dictionary<string, Assembly> _assemblies;
        private readonly HashSet<string> _executableExtensions;

        public AssemblyManager()
        {
            _assemblies = new Dictionary<string, Assembly>();
            _executableExtensions = new HashSet<string> {".dll", ".exe"};
            AddAssemblies();
        }

        public void AddAssembly<T>()
        {
            AddAssembly(typeof(T).Assembly);
        }

        public IEnumerable<Assembly> GetAssemblies()
        {
            return _assemblies.Values.ToList();
        }

        private void AddAssembly(Assembly assembly)
        {
            var assemblyName = assembly.FullName;
            if (string.IsNullOrWhiteSpace(assemblyName) || _assemblies.ContainsKey(assemblyName))
                return;

            _assemblies.Add(assemblyName, assembly);
        }

        private void AddAssemblies()
        {
            var entryLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            if (string.IsNullOrWhiteSpace(entryLocation) == false) AddAssemblies(entryLocation);

            var callingLocation = Path.GetDirectoryName(Assembly.GetCallingAssembly()?.Location);
            if (string.IsNullOrWhiteSpace(callingLocation) == false) AddAssemblies(callingLocation);

            var executingLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly()?.Location);
            if (string.IsNullOrWhiteSpace(executingLocation) == false) AddAssemblies(executingLocation);
        }

        private void AddAssemblies(string location)
        {
            if (string.IsNullOrWhiteSpace(location)) return;

            var files = Directory.GetFiles(location);
            foreach (var file in files)
            {
                try
                {
                    if (IsExecutable(file) == false)
                        continue;

                    var assemblyName = Path.GetFileNameWithoutExtension(file);
                    if (string.IsNullOrWhiteSpace(assemblyName))
                        continue;

                    var assembly = Assembly.Load(assemblyName);
                    AddAssembly(assembly);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private bool IsExecutable(string file)
        {
            var fileInfo = new FileInfo(file);
            var extension = fileInfo.Extension;
            return _executableExtensions.Any(execExtension => IsEqual(extension, execExtension));
        }

        private bool IsEqual(string extension, string executableExtension)
        {
            return extension.Equals(executableExtension, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}