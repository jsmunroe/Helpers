using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helpers.Test
{
    [TestClass]
    public class ScopeProviderTest
    {
        [TestMethod]
        public void ConstructScopeProvider()
        {
            // Execute
            new ScopeProvider(ConstructScopeProvider);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructScopeProviderWithNullAction()
        {
            // Execute
            new ScopeProvider(a_disposeAction: null);
        }

        [TestMethod]
        public void DisposeScopeProvider()
        {
            // Setup
            var called = false;
            Action action = () => called = true;
            var scopeProvider = new ScopeProvider(action);

            // Execute
            scopeProvider.Dispose();

            // Assert
            Assert.IsTrue(called);
        }


    }
}
