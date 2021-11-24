﻿namespace Tethos.NSubstitute
{
    using System;
    using Castle.Core;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Context;
    using Castle.MicroKernel.Handlers;
    using Castle.MicroKernel.Registration;
    using global::NSubstitute;
    using global::NSubstitute.Core;
    using Tethos.Extensions;

    /// <inheritdoc />
    internal class AutoNSubstituteResolver : AutoResolver
    {
        /// <inheritdoc cref="AutoResolver" />
        public AutoNSubstituteResolver(IKernel kernel)
            : base(kernel)
        {
        }

        /// <inheritdoc />
        public override bool CanResolve(
            CreationContext context,
            ISubDependencyResolver contextHandlerResolver,
            ComponentModel model,
            DependencyModel dependency) => dependency.TargetType.IsClass || base.CanResolve(context, contextHandlerResolver, model, dependency);

        /// <inheritdoc />
        public override object MapToTarget(Type targetType, Arguments constructorArguments)
        {
            var arguments = targetType.IsInterface switch
            {
                true => Array.Empty<object>(),
                false => constructorArguments.Flatten(),
            };
            var mock = Substitute.For(new Type[] { targetType }, arguments);
            var func = () => this.Kernel.Resolve(targetType);
            var currentObject = func.SwallowExceptions(typeof(ComponentNotFoundException), typeof(HandlerException));

            if (currentObject is not ICallRouterProvider)
            {
                this.Kernel.Register(Component.For(targetType)
                    .Instance(mock)
                    .OverridesExistingRegistration());
            }

            return mock;
        }
    }
}
