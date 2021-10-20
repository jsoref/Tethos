﻿using AutoFixture.Xunit2;
using Castle.MicroKernel;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace Tethos.Moq.Tests
{
    public class AutoMoqResolverTests
    {
        [Theory, AutoData]
        public void DiamondType_ShouldResolveToMoqType(Mock<IKernel> kernel)
        {
            // Arrange
            var sut = new AutoMoqResolver(kernel.Object);
            var expected = typeof(Mock<>).GetType();

            // Act
            var actual = sut.DiamondType;

            // Assert
            actual.Should().BeOfType(expected);
        }

        [Theory, AutoData]
        public void MapToTarget_ShouldReturnMock(Mock<object> expected, Mock<IKernel> kernel, Type targetType)
        {
            // Arrange
            var sut = new AutoMoqResolver(kernel.Object);

            // Act
            var actual = sut.MapToTarget(expected, targetType);

            // Assert
            actual.Should().Equals(expected);
        }
    }
}
