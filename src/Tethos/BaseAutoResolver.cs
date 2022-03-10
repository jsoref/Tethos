﻿namespace Tethos
{
    using System.Linq;
    using Castle.Core;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Context;
    using Castle.MicroKernel.Handlers;
    using Castle.MicroKernel.Resolvers;
    using Tethos.Extensions;

    /// <summary>
    /// Auto mocking resolver abstraction.
    /// </summary>
    internal abstract class BaseAutoResolver : ISubDependencyResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAutoResolver"/> class.
        /// </summary>
        /// <param name="kernel">Reference to Castle Container.</param>
        protected BaseAutoResolver(IKernel kernel) => this.Kernel = kernel;

        /// <summary>
        /// Gets <see cref="Castle.Windsor"/> kernel dependency.
        /// </summary>
        public IKernel Kernel { get; }

        /// <summary>
        /// Maps target mock object to mocked object type.
        /// </summary>
        /// <param name="argument">Argument object used to pass mapping parameters.</param>
        /// <returns>Auto-mocked object depending on target type.</returns>
        public abstract object MapToMock(MockMapping argument);

        /// <inheritdoc />
        public virtual bool CanResolve(
            CreationContext context,
            ISubDependencyResolver contextHandlerResolver,
            ComponentModel model,
            DependencyModel dependency) => dependency.TargetType.IsInterface;

        /// <inheritdoc />
        public object Resolve(
            CreationContext context,
            ISubDependencyResolver contextHandlerResolver,
            ComponentModel model,
            DependencyModel dependency)
        {
            var targetType = dependency.TargetType;
            var getTargetObject = () => this.Kernel.Resolve(targetType);

            // TODO: Extract exception types to a variable
            var currentTargetObject = getTargetObject.SwallowExceptions(typeof(ComponentNotFoundException), typeof(HandlerException), typeof(DependencyResolverException));
            var arguments = context.AdditionalArguments
                .Where(_ => !targetType.IsInterface)
                .Where(argument => argument.Key.GetArgumentType() == $"{targetType}");
            var constructorArguments = new Arguments().Add(arguments);

            return this.MapToMock(new()
            {
                TargetType = targetType,
                TargetObject = currentTargetObject,
                ConstructorArguments = constructorArguments,
            });
        }
    }
}
