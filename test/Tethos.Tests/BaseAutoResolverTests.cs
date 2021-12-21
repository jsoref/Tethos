﻿namespace Tethos.Tests
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Threading.Tasks;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Context;
    using FluentAssertions;
    using Moq;
    using Tethos.Tests.Attributes;
    using Tethos.Tests.SUT;
    using Xunit;

    public class BaseAutoResolverTests
    {
        [Theory]
        [InlineAutoMoqData(typeof(IList), true)]
        [InlineAutoMoqData(typeof(IEnumerable), true)]
        [InlineAutoMoqData(typeof(Array), false)]
        [InlineAutoMoqData(typeof(Enumerable), false)]
        [InlineAutoMoqData(typeof(Type), false)]
        [InlineAutoMoqData(typeof(BaseAutoResolver), false)]
        [InlineAutoMoqData(typeof(TimeoutException), false)]
        [InlineAutoMoqData(typeof(Guid), false)]
        [InlineAutoMoqData(typeof(Task<>), false)]
        [InlineAutoMoqData(typeof(Task<int>), false)]
        [InlineAutoMoqData(typeof(int), false)]
        [Trait("Category", "Unit")]
        public void CanResolve_ShouldMatch(
            Type type,
            bool expected,
            IKernel kernel,
            CreationContext resolver,
            string key)
        {
            // Arrange
            var sut = new ConcreteAutoResolver(kernel);

            // Act
            var actual = sut.CanResolve(
                resolver,
                resolver,
                new(),
                new(key, type, false));

            // Assert
            actual.Should().Be(expected);
        }

        [Theory]
        [AutoMoqData]
        [Trait("Category", "Unit")]
        public void Resolve_ShouldMatch(
            Mock<IKernel> kernel,
            Mock<object> expected,
            CreationContext resolver,
            string key)
        {
            // Arrange
            var type = expected.GetType();
            kernel.Setup(mock => mock.Resolve(type)).Returns(expected);
            var sut = new ConcreteAutoResolver(kernel.Object);

            // Act
            var actual = sut.Resolve(
                resolver,
                resolver,
                new(),
                new(key, type, false));

            // Assert
            actual.Should().BeSameAs(expected);
        }

        [Theory]
        [InlineAutoMoqData("pattern")]
        [Trait("Category", "Unit")]
        public void Resolve_Arguments_ShouldMatch(
            string pattern,
            Mock<IKernel> kernel,
            Mock<object> expected,
            CreationContext resolver,
            string key)
        {
            // Arrange
            var type = expected.GetType();
            kernel.Setup(mock => mock.Resolve(type)).Returns(expected);
            var sut = new MockedAutoResolver(kernel.Object);

            resolver.AdditionalArguments.Add(new Arguments().AddNamed($"{type}__name", "foo"));

            // Act
            var actual = sut.Resolve(
                resolver,
                resolver,
                new(),
                new(key, type, false));

            // Assert
            (actual as MapToMockArgs).ConstructorArguments.Should().HaveSameCount(resolver.AdditionalArguments);
            (actual as MapToMockArgs).TargetType.Should().Be(type);
            (actual as MapToMockArgs).TargetObject.Should().Be(expected);

        }
    }
}
