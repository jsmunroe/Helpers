using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Helpers.Test.Archetecture
{
    [TestClass]
    public class ResultTest
    {
        [TestMethod]
        public void SetSuccess()
        {
            // Setup
            var result = new Result
            {
                Success = false,
                Message = "",
                Exception = null,
            };

            // Execute
            result.Success = true;

            // Assert 
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual("", result.Message);
            Assert.AreEqual(null, result.Exception);
        }

        [TestMethod]
        public void SetMessage()
        {
            // Setup
            var result = new Result
            {
                Success = false,
                Message = "",
                Exception = null,
            };

            // Execute
            result.Message = "Uh oh!";

            // Assert 
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual("Uh oh!", result.Message);
            Assert.AreEqual(null, result.Exception);
        }


        [TestMethod]
        public void SetException()
        {
            // Setup
            var result = new Result
            {
                Success = false,
                Message = "",
                Exception = null,
            };

            // Execute
            var exception = new Exception();
            result.Exception = exception;

            // Assert 
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual("", result.Message);
            Assert.AreSame(exception, result.Exception);
        }


        [TestMethod]
        public void GetOK()
        {
            // Execute
            var result = Result.OK;

            // Assert
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual("", result.Message);
            Assert.AreEqual(null, result.Exception);
        }


        [TestMethod]
        public void Fail()
        {
            // Execute
            var result = Result.Fail();

            // Assert
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual("", result.Message);
            Assert.AreEqual(null, result.Exception);
        }

        [TestMethod]
        public void FailWithMessage()
        {
            // Execute
            var result = Result.Fail("Aw, man!");

            // Assert
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual("Aw, man!", result.Message);
            Assert.AreEqual(null, result.Exception);
        }

        [TestMethod]
        public void FailWithException()
        {
            // Execute
            var exception = new Exception("Bad news!");
            var result = Result.Fail(exception);

            // Assert
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual("Bad news!", result.Message);
            Assert.AreSame(exception, result.Exception);
        }

        [TestMethod]
        public void FailWithNullMessage()
        {
            // Execute
            var result = Result.Fail(a_message: null);

            // Assert
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual("", result.Message);
            Assert.AreEqual(null, result.Exception);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FaileWithNullException()
        {
            // Execute
            Result.Fail(a_exception: null);
        }

        [TestMethod]
        public void CastFromBool()
        {
            // Execute
            Result result = true;

            // Assert
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual("", result.Message);
            Assert.AreEqual(null, result.Exception);
        }

        [TestMethod]
        public void CastToBool()
        {
            // Execute
            bool result = Result.OK;

            // Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void CastFromErrorMessage()
        {
            // Execute
            var result = (Result)"Darn it!";

            // Assert
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual("Darn it!", result.Message);
            Assert.AreEqual(null, result.Exception);
        }


        [TestMethod]
        public void CastFromException()
        {
            // Execute
            var exception = new Exception("Well, shucks!");
            var result = (Result)exception;

            // Assert
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual("Well, shucks!", result.Message);
            Assert.AreSame(exception, result.Exception);
        }

    }
}
