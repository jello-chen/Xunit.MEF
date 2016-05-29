using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Xunit.MEF
{
    /// <summary>
    /// Implements a mef dependency resolver bootstrapper
    /// </summary>
    public class MefDependencyResolverBootstrapper : IDependencyResolverBootstrapper
    {

        public static readonly IDependencyResolver DependencyResolver;

        static MefDependencyResolverBootstrapper()
        {
            var aggregateCatalog = new AggregateCatalog();
            var dlls = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            foreach (var dll in dlls)
            {
                try
                {
                    var catalog = new AssemblyCatalog(Assembly.LoadFrom(dll));
                    var parts = catalog.Parts.ToArray();//if loading the assembly fails,throw a load exception
                    aggregateCatalog.Catalogs.Add(catalog);
                }
                catch
                {
                    
                }
            }
            var container = new CompositionContainer(aggregateCatalog);
            DependencyResolver = new MefDependencyResolver(container);
        }


        public IDependencyResolver GetResolver()
        {
            return DependencyResolver;
        }
    }
}
