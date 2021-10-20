﻿using AutoFixture.Xunit2;
using FakeItEasy;
using Tethos.FakeItEasy.Tests.SUT;
using Xunit;

namespace Tethos.FakeItEasy.Tests
{
    public class InheritedAutoMockingTestTests : AutoMockingTest
    {
        [Theory, AutoData]
        public void Dispose_ShouldDisposeMock(InheritedAutoMockingTest sut)
        {
            // Act
            sut.Dispose();

            // Assert
            A.CallTo(() => sut.Container.Dispose()).MustHaveHappened();
        }
    }
}
