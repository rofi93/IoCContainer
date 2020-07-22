using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace IoCContainer.IoC
{
    public class TypeManager : ITypeManager
    {
        private readonly HashSet<Type> _exportedTypes;
        private readonly HashSet<string> _executableExtensions;

        public TypeManager()
        {
            _exportedTypes = new HashSet<Type>();
            _executableExtensions = new HashSet<string> {".dll", ".exe"};
            AddAssemblies();
        }

        public void AddExportedTypes(Assembly assembly)
        {
            _exportedTypes.UnionWith(assembly.ExportedTypes);
        }

        public void AddType<T>()
        {
            _exportedTypes.Add(typeof(T));
        }

        public IEnumerable<Type> GetExportedTypes()
        {
            return _exportedTypes.ToList();
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
                    AddExportedTypes(assembly);
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