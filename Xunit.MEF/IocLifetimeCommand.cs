using System;
using Xunit.Sdk;

namespace Xunit.MEF
{
    /// <summary>
    /// A <see cref="ITestCommand"/> that wraps any other <see cref="ITestCommand"/>
    /// and resolves the test class instance from the container.
    /// </summary>
    /// <remarks>
    /// These are manufactured by the <see cref="IocTestClassCommand"/>.
    /// </remarks>
    public class IocLifetimeCommand : TestCommand
    {
        private readonly TestCommandWrapper _wrapper;

        public IocLifetimeCommand(TestCommandWrapper wrapper) 
            : base(wrapper.Method, wrapper.DisplayName, wrapper.Timeout)
        {
            _wrapper = wrapper;
        }

        /// <inheritdoc/>
        public override bool ShouldCreateInstance
        {
            get { return false; } //We're creating the instance out of the container
        }

        /// <inheritdoc/>
        public override MethodResult Execute(object testClass)
        {
            if (testClass != null)
                throw new InvalidOperationException("testClass is unexpectedly not null");

            var dependencyResolver = ContainerResolver.GetDependencyResolver(testMethod.Class.Type);
            using (var lifetimeScope = dependencyResolver.CreateScope())
            {
                testClass = lifetimeScope.GetType(testMethod.Class.Type);
                return _wrapper.Execute(testClass);
            }
        }
    }
}
