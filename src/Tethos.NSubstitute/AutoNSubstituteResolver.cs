﻿using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using NSubstitute;
using System;
using System.Linq;

namespace Tethos.NSubstitute
{
    /// <summary>
    /// <see cref="AutoResolver"/> tailored for <see cref="NSubstitute"/> mocking systems.
    /// </summary>
    public class AutoNSubstituteResolver : AutoResolver
    {
        /// <inheritdoc />
        public AutoNSubstituteResolver(IKernel kernel) : base(kernel)
        {
        }

        /// <inheritdoc />
        public override bool CanResolve(CreationContext context, ISubDependencyResolver contextHandlerResolver, ComponentModel model, DependencyModel dependency)
            => dependency.TargetType.IsClass || base.CanResolve(context, contextHandlerResolver, model, dependency);

        /// <inheritdoc />
        public override object MapToTarget(Type targetType, CreationContext context)
        {
            // TODO: Need to implement argument separation logic
            var arguments = context.AdditionalArguments
                .Select(argument => argument.Value)
                .ToArray();

            var mock = Substitute.For(new Type[] { targetType }, arguments);

            Kernel.Register(Component.For(targetType)
                .Instance(mock)
                .OverridesExistingRegistration()
            );

            return mock;
        }
    }
}
