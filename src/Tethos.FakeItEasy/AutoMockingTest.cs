﻿using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FakeItEasy;

namespace Tethos.FakeItEasy
{
    /// <summary>
    /// <see cref="Tethos"/> auto-mocking system using <see cref="FakeItEasy"/> to inject mocks.
    /// </summary>
    public class AutoMockingTest : BaseAutoMockingTest<AutoFakeItEasyContainer>
    {
        /// <inheritdoc />
        public override void Install(IWindsorContainer container, IConfigurationStore store)
        {
            AutoResolver = new AutoFakeItEasyResolver(container.Kernel);

            container.Kernel.Resolver.AddSubResolver(
                AutoResolver
            );

            container.Register(Component.For(typeof(Fake<>)));

            base.Install(container, store);
        }
    }
}
