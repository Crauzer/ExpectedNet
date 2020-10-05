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
        [Test] public void TestMapOr()
        {
            var resultOk = Result<int, string>.Ok(0);
            var resultError = Result<int, string>.Error("kek");

            Assert.AreEqual(5, resultOk.MapOr(20, (int x) => { return x + 5; }));
            Assert.AreEqual(20, resultError.MapOr(20, (int x) => { return x + 5; }));
        }
        [Test] public void TestMapOrElse()
        {
            var resultOk = Result<int, string>.Ok(0);
            var resultError = Result<int, string>.Error("kek");

            Assert.AreEqual(2, resultOk.MapOrElse((string error) => { return 1; }, (int x) => { return 2; }));
            Assert.AreEqual(1, resultError.MapOrElse((string error) => { return 1; }, (int x) => { return 2; }));
        }

        [Test] public void TestAnd()
        {
            var resultOk = Result<int, string>.Ok(0);
            var resultError = Result<int, string>.Error("kek");

            Assert.AreEqual(5, resultOk.And(Result<int, string>.Ok(5)).Unwrap());
            Assert.IsTrue(resultError.And(Result<int, string>.Ok(5)).IsError());
        }
        [Test] public void TestAndThen()
        {
            var resultOk = Result<int, string>.Ok(0);
            var resultError = Result<int, string>.Error("kek");

            Assert.AreEqual(6, resultOk.AndThen((int x) =>
            {
                return Result<int, string>.Ok(x + 6);
            }).Unwrap());

            Assert.IsTrue(resultError.AndThen((int x) =>
            {
                return Result<int, string>.Ok(x + 6);
            }).IsError());
        }
    }
}