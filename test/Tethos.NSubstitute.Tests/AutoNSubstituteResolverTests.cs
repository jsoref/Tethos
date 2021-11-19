﻿using Castle.MicroKernel;
using FluentAssertions;
using Tethos.NSubstitute.Tests.Attributes;
using Tethos.Tests.Common;
using Xunit;

namespace Tethos.NSubstitute.Tests
{
    public class AutoNSubstituteResolverTests
    {
        [Theory, AutoNSubstituteData]
        [Trait("", "Internal")]
        public void MapToTarget_ShouldMatchMockedType(
            IMockable mockable,
            IKernel kernel,
            Arguments constructorArguments
        )
        {
            // Arrange
            var sut = new AutoNSubstituteResolver(kernel);
            var expected = mockable.GetType();
            var type = typeof(IMockable);

            // Act
            var actual = sut.MapToTarget(type, constructorArguments);

            // Assert
            actual.Should().BeOfType(expected);
        }
    }
}
