using ExpectedNet.Extensions.Result;
using NUnit.Framework;
using System;

using static ExpectedNet.Extensions.Result.ResultExtensions;

namespace ExpectedNet.Tests
{
    [TestFixture(Author = "Crauzer")]
    public class ResultTests
    {
        [Test]
        public void TestIsOk()
        {
            var result = Expect(0, "error");

            Assert.IsTrue(result.IsOk());
        }
        [Test]
        public void TestIsError()
        {
            var result = Expect<string, string>(null, "cats > dogs");

            Assert.IsTrue(result.IsError());
        }

        [Test]
        public void TestGetOk()
        {
            var result = Expect(0, "error");
            var ok = result.GetOk();

            Assert.IsTrue(ok.HasValue);

        }
        [Test]
        public void TestGetError()
        {
            var result = Expect<string, string>(null, "420");
            var error = result.GetError();

            Assert.IsTrue(error.HasValue);
        }

        [Test]
        public void TestUnwrap()
        {
            var resultOk = Expect(0, "error");
            var resultError = Expect<string, string>(null, "error");

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
        [Test]
        public void TestUnwrapOr()
        {
            var resultOk = Expect(0, "error");
            var resultError = Expect<int?, string>(null, "error");

            Assert.AreEqual(0, resultOk.UnwrapOr(5));
            Assert.AreEqual(5, resultError.UnwrapOr(5));
        }
        [Test]
        public void TestUnwrapOrElse()
        {
            var resultOk = Expect(0, "error");
            var resultError = Expect<int?, string>(null, "error");

            Assert.AreEqual(0, resultOk.UnwrapOrElse((string error) =>
            {
                return 56;
            }));

            Assert.AreEqual(56, resultError.UnwrapOrElse((string error) =>
            {
                return 56;
            }));
        }

        [Test]
        public void TestMap()
        {
            var resultOk = Expect(0, "error");
            var resultError = Expect<int?, string>(null, "error");

            Assert.AreEqual(1, resultOk.Map((int x) => { return x + 1; }).Unwrap());
            Assert.IsTrue(resultError.Map((int? x) => { return x + 1; }).IsError());
        }
        [Test]
        public void TestMapOr()
        {
            var resultOk = Expect(0, "error");
            var resultError = Expect<int?, string>(null, "error");

            Assert.AreEqual(5, resultOk.MapOr(20, (int x) => { return x + 5; }));
            Assert.AreEqual(20, resultError.MapOr(20, (int? x) => { return x + 5; }));
        }
        [Test]
        public void TestMapOrElse()
        {
            var resultOk = Expect(0, "error");
            var resultError = Expect<int?, string>(null, "error");

            Assert.AreEqual(2, resultOk.MapOrElse((string error) => { return 1; }, (int x) => { return 2; }));
            Assert.AreEqual(1, resultError.MapOrElse((string error) => { return 1; }, (int? x) => { return 2; }));
        }

        [Test]
        public void TestAnd()
        {
            var resultOk = Expect(0, "error");
            var resultError = Expect<int?, string>(null, "error");

            Assert.AreEqual(
                5,
                resultOk
                .And(Expect(5, "error"))
                .Unwrap());
            Assert.IsTrue(
                resultError
                .And(Expect<int?, string>(null, "error"))
                .IsError());
        }
        [Test]
        public void TestAndThen()
        {
            var resultOk = Expect(0, "error");
            var resultError = Expect<int?, string>(null, "error");

            Assert.AreEqual(6, resultOk.AndThen((int x) =>
            {
                return Expect(x + 6, "error");
            }).Unwrap());

            Assert.IsTrue(resultError.AndThen((int? x) =>
            {
                return Expect(x + 6, "error");
            }).IsError());
        }

        [Test]
        public void TestOr()
        {
            var resultOk = Expect(0, "error");
            var resultError = Expect<int?, string>(null, "error");

            Assert.AreEqual(
                0,
                resultOk.Or(Expect(5, "error"))
                .Unwrap());
            Assert.AreEqual(5,
                resultError
                .Or(Expect((int?)5, "error"))
                .Unwrap());
        }
        [Test]
        public void TestOrElse()
        {
            var resultOk = Expect(0, "error");
            var resultError = Expect<int?, string>(null, "error");

            Assert.AreEqual(0, resultOk.OrElse((string error) =>
            {
                return Expect(10, "error");
            }).Unwrap());
            Assert.AreEqual(10, resultError.OrElse((string error) =>
            {
                return Expect((int?)10, "error");
            }).Unwrap());
        }
    
        [Test]
        public void TestCatch()
        {
            var resultOk = Result<int, Exception>.Catch(() => 
            {
                return 5;
            });

            var resultError = Result<int, Exception>.Catch(() =>
            {
                throw new InvalidOperationException("error");
                return 10; // for type inferring
            });

            Assert.IsTrue(resultOk.IsOk());
            Assert.AreEqual(5, resultOk.Unwrap());

            Assert.IsTrue(resultError.IsError());
        }
    }
}