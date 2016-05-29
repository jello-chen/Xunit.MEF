using System;
using System.Collections.Generic;
using System.Linq;

namespace Xunit.MEF
{
    /// <summary>
    /// Resolves container
    /// </summary>
    class ContainerResolver
    {
        /// <summary>
        /// Gets IOC Container
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IDependencyResolver GetDependencyResolver(Type type)
        {
            var containerBootstrapperAttribute =
                    type
                        .GetCustomAttributes(typeof(DependencyResolverBootstrapperAttribute), false)
                        .Cast<DependencyResolverBootstrapperAttribute>()
                        .FirstOrDefault()
                    ??
                    type.Assembly
                        .GetCustomAttributes(typeof(DependencyResolverBootstrapperAttribute), false)
                        .Cast<DependencyResolverBootstrapperAttribute>()
                        .FirstOrDefault();

            if (containerBootstrapperAttribute == null)
                throw new InvalidOperationException("Cannot find an DependencyResolverBootstrapperAttribute on either the test assembly or class");

            var bootstrapper = (IDependencyResolverBootstrapper)Activator.CreateInstance(containerBootstrapperAttribute.BootstrapperType);
            return bootstrapper.GetResolver();
        }
    }
}
