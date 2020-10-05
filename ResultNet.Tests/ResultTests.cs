using NUnit.Framework;
using System;

namespace ResultNet.Tests
{
    [TestFixture(Author = "Crauzer")]
    public class ResultTests
    {
        [Test] public void TestIsOk()
        {
            var result = Result<int, string>.Ok(0);

            Assert.IsTrue(result.IsOk());
        }
        [Test] public void TestIsError()
        {
            var result = Result<int, string>.Error("cats > dogs");

            Assert.IsTrue(result.IsError());
        }

        [Test] public void TestGetOk()
        {
            var result = Result<int, string>.Ok(0);
            var ok = result.GetOk();

            Assert.IsTrue(ok.HasValue);

        }
        [Test] public void TestGetError()
        {
            var result = Result<int, string>.Error("420");
            var error = result.GetError();

            Assert.IsTrue(error.HasValue);
        }
    
        [Test] public void TestUnwrap()
        {
            var resultOk = Result<int, string>.Ok(0);
            var resultError = Result<int, string>.Error("kek");

            Assert.DoesNotThrow(() => 
            {
                resultOk.Unwrap();
            });

            Assert.AreEqual(0, resultOk.Unwrap());

            Assert.Throws(typeof(InvalidOperationException), () => 
            {
                resultError.Unwrap();
            });
        }
        [Test] public void TestUnwrapOr()
        {
            var resultOk = Result<int, string>.Ok(0);
            var resultError = Result<int, string>.Error("kek");

            Assert.AreEqual(0, resultOk.UnwrapOr(5));
            Assert.AreEqual(5, resultError.UnwrapOr(5));
        }
        [Test] public void TestUnwrapOrElse()
        {
            var resultOk = Result<int, string>.Ok(0);
            var resultError = Result<int, string>.Error("kek");

            Assert.AreEqual(0, resultOk.UnwrapOrElse((string error) =>
            {
                return 56;
            }));

            Assert.AreEqual(56, resultError.UnwrapOrElse((string error) =>
            {
                return 56;
            }));
        }

        [Test] public void TestMap()
        {
            var resultOk = Result<int, string>.Ok(0);
            var resultError = Result<int, string>.Error("kek");

            Assert.AreEqual(1, resultOk.Map((int x) => { return x + 1; }).Unwrap());
            Assert.IsTrue(resultError.Map((int x) => { return x + 1; }).IsError());
        }
    }
}