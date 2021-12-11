﻿namespace Tethos.NSubstitute
{
    using System;
    using Castle.Core;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Context;
    using Castle.MicroKernel.Registration;
    using global::NSubstitute;
    using global::NSubstitute.Core;
    using Tethos.Extensions;

    /// <inheritdoc />
    internal class AutoResolver : BaseAutoResolver
    {
        /// <inheritdoc cref="BaseAutoResolver" />
        public AutoResolver(IKernel kernel)
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
        public override object MapToMock(Type targetType, object targetObject, Arguments constructorArguments)
        {
            var arguments = targetType.IsInterface switch
            {
                true => Array.Empty<object>(),
                false => constructorArguments.Flatten(),
            };
            var mock = Substitute.For(new[] { targetType }, arguments);
            var isPlainObject = targetObject is not ICallRouterProvider;

            if (isPlainObject)
            {
                this.Kernel.Register(Component.For(targetType)
                    .Instance(mock)
                    .OverridesExistingRegistration());
            }

            return mock;
        }
    }
}
