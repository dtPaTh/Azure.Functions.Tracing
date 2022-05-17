using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Azure.Functions.Tracing.Internal
{
    internal static class AssemblyExtensions
    {
        internal static IEnumerable<Assembly> RelatedTo(this IEnumerable<Assembly> assemblies, Assembly assembly)
        {
            return assemblies.Where(
                p => !p.IsDynamic && (
                    p.FullName == assembly.FullName ||
                    p.GetReferencedAssemblies().Any(q => q.FullName == assembly.FullName)
                )
            );
        }

        internal static IEnumerable<Assembly> Matches(this IEnumerable<Assembly> assemblies, string name)
        {
            return assemblies.Where(
                p => !p.IsDynamic && (
                    p.FullName.Contains(name) ||
                    p.GetReferencedAssemblies().Any(q => q.FullName.Contains(name))
                )
            );
        }

        internal static IEnumerable<Assembly> Matches(this IEnumerable<Assembly> assemblies, IEnumerable<string> name)
        {
            return assemblies.Where(
                p => !p.IsDynamic && (
                    name.Any(n => p.FullName.Contains(n)) ||
                    p.GetReferencedAssemblies().Any(q => name.Any(n => q.FullName.Contains(n)))
                )
            );
        }
    }
}
