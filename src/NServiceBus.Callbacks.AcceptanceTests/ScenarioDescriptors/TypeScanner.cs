﻿namespace NServiceBus.AcceptanceTests.ScenarioDescriptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using NServiceBus.Hosting.Helpers;

    public class TypeScanner
    {
        public static IEnumerable<Type> GetAllTypesAssignableTo<T>()
        {
            return AvailableAssemblies.SelectMany(a => a.GetTypes())
                .Where(t => typeof(T).IsAssignableFrom(t) && t != typeof(T))
                .ToList();
        }

        static IEnumerable<Assembly> AvailableAssemblies
        {
            get
            {
                if (assemblies == null)
                {
                    var result = new AssemblyScanner().GetScannableAssemblies();

                    assemblies = result.Assemblies.Where(a =>
                    {
                        var references = a.GetReferencedAssemblies();

                        return references.All(an => an.Name != "nunit.framework");
                    }).ToList();
                }

                return assemblies;
            }
        }

        static List<Assembly> assemblies;
    }
}