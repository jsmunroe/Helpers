using System;

namespace Helpers.Archetecture
{
    public class Result
    {
        /// <summary>
        /// Result successfulness.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Result message.
        /// </summary>
        public string Message { get; set; } = "";

        /// <summary>
        /// Result exception.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Successful result. 
        /// </summary>
        public static Result OK { get; } = new Result { Success = true };

        /// <summary>
        /// Create failed result with the given message (<paramref name="a_message"/>).
        /// </summary>
        /// <param name="a_message">Failure message.</param>
        /// <returns>Failed result.</returns>
        public static Result Fail(string a_message = "")
        {
            return new Result
            {
                Success = false,
                Message = a_message ?? ""
            };
        }

        /// <summary>
        /// Create a failed result with the given exception (<paramref name="a_exception"/>).
        /// </summary>
        /// <param name="a_exception">Exception.</param>
        /// <returns>Failed result.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="a_exception"/> is null.</exception>
        public static Result Fail(Exception a_exception)
        {
            #region Argument Validation

            if (a_exception == null)
                throw new ArgumentNullException(nameof(a_exception));

            #endregion

            return new Result
            {
                Success = false,
                Message = a_exception.Message,
                Exception = a_exception
            };
        }

        /// <summary>
        /// Cast a result from the given error message (<paramref name="a_message"/>).
        /// </summary>
        /// <param name="a_message">Error message.</param>
        public static explicit operator Result(string a_message)
        {
            return Fail(a_message);
        }

        /// <summary>
        /// Cast a result from the given exception (<paramref name="a_exception"/>).
        /// </summary>
        /// <param name="a_exception">Error message.</param>
        public static explicit operator Result(Exception a_exception)
        {
            return Fail(a_exception);
        }

        /// <summary>
        /// Cast a result from the given success value (<paramref name="a_success"/>).
        /// </summary>
        /// <param name="a_success">Success value.</param>
        public static implicit operator Result(bool a_success)
        {
            return new Result {Success = true};
        }

        /// <summary>
        /// Cast the given result (<paramref name="a_result"/>) to a boolean value.
        /// </summary>
        /// <param name="a_result">Result.</param>
        public static implicit operator bool(Result a_result)
        {
            return a_result.Success;
        }
    }
}