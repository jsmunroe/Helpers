using System;

namespace Helpers
{
    public class ScopeProvider : IDisposable
    {
        private Action _disposeAction;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="a_disposeAction">Action invoked when this provider is disposed.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_disposeAction"/> is null.</exception>
        public ScopeProvider(Action a_disposeAction)
        {
            #region Argument Validation

            if (a_disposeAction == null)
                throw new ArgumentNullException(nameof(a_disposeAction));

            #endregion

            _disposeAction = a_disposeAction;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _disposeAction?.Invoke();
            _disposeAction = null;
        }
    }
}