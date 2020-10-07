using System;

namespace ExpectedNet.Extensions.Result
{
    /// <summary>
    /// Contains extension methods for <see cref="Result{T, E}"/>
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Creates a new <see cref="Ok{T, E}"/> containing <paramref name="value"/> if it isn't null,
        /// otherwise returns an <see cref="Error{T, E}"/> containing the default value of <typeparamref name="E"/>
        /// </summary>
        public static Result<T, E> Expect<T, E>(T value)
        {
            if (value is null)
            {
                return new Error<T, E>(default);
            }
            else
            {
                return new Ok<T, E>(value);
            }
        }

        /// <summary>
        /// Creates a new <see cref="Ok{T, E}"/> containing <paramref name="value"/> if it isn't null,
        /// otherwise returns an <see cref="Error{T, E}"/> containing <paramref name="error"/> which cannot be null
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static Result<T, E> Expect<T, E>(T value, E error)
        {
            if (error is null) throw new ArgumentNullException(nameof(error), "cannot be null");

            if (value is null)
            {
                return new Error<T, E>(error);
            }
            else
            {
                return new Ok<T, E>(value);
            }
        }

        /// <summary>
        /// Creates a new <see cref="Ok{T, E}"/> containing <paramref name="value"/> if it isn't null,
        /// otherwise returns an <see cref="Error{T, E}"/> containing the default value of <typeparamref name="E"/>
        /// </summary>
        public static Result<T, E> ExpectValue<T, E>(this T value)
        {
            if (value is null)
            {
                return new Error<T, E>(default);
            }
            else
            {
                return new Ok<T, E>(value);
            }
        }

        /// <summary>
        /// Creates a new <see cref="Ok{T, E}"/> containing <paramref name="value"/> if it isn't null,
        /// otherwise returns an <see cref="Error{T, E}"/> containing <paramref name="error"/> which cannot be null
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static Result<T, E> ExpectValue<T, E>(this T value, E error)
        {
            if (error is null) throw new ArgumentNullException(nameof(error), "cannot be null");

            if (value is null)
            {
                return new Error<T, E>(error);
            }
            else
            {
                return new Ok<T, E>(value);
            }
        }

        /// <summary>
        /// Calls <paramref name="func"/> catching any <see cref="Exception"/> it throws into an <see cref="Error{T, E}"/>,
        /// if no <see cref="Exception"/> is thrown, an <see cref="Ok{T, E}"/> value is returned
        /// </summary>
        public static Result<TResult, Exception> Catch<TResult>(Func<TResult> func)
        {
            return Result<TResult, Exception>.Catch(func);
        }

        /// <summary>
        /// Returns a new <see cref="Unit"/>
        /// </summary>
        public static Unit Void()
        {
            return new Unit();
        }
    }
}
