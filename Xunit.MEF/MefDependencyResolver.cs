using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xunit.MEF
{
    /// <summary>
    /// Implements a mef dependency resolver
    /// </summary>
    public class MefDependencyResolver : IDependencyResolver
    {
        private readonly CompositionContainer _container;

        public MefDependencyResolver(CompositionContainer container)
        {
            _container = container;
        }

        public IDependencyScope CreateScope()
        {
            return this;
        }

        public void Dispose()
        {
            
        }

        public object GetType(Type type)
        {
            var contractName = AttributedModelServices.GetContractName(type);
            return _container.GetExportedValueOrDefault<object>(contractName);
        }
    }
}
