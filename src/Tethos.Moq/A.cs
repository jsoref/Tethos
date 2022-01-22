﻿namespace Tethos.Moq
{
    /// <summary>
    /// Static entry-point for generating <see cref="IAutoMockingContainer"/> containers used for auto-mocking.
    /// </summary>
    public static class A
    {
        /// <summary>
        /// Gets ready to use auto-mocking container.
        /// </summary>
        public static IAutoMockingContainer Container
        {
            get
            {
                if (Singleton == null)
                {
                    Singleton = new AutoMockingTest().Container;
                }

                return Singleton;
            }
        }

        private static IAutoMockingContainer Singleton { get; set; }
    }
}
