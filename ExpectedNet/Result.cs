using Optional;
using System;

// Disable warning because we do not allow users to derive from Result
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).

namespace ExpectedNet
{
    /// <summary>
    /// Represents a type which can be either <see cref="Ok{T, E}"/> or <see cref="Error{T, E}"/>
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Ok{T, E}"/> value</typeparam>
    /// <typeparam name="E">The type of the <see cref="Error{T, E}"/> value</typeparam>
    public abstract class Result<T, E>
    {
        // Internal constructor to prevent users from deriving from this type
        internal Result()
        {

        }

        /// <summary>
        /// Creates a new <see cref="Ok{T, E}"/> result containing <paramref name="value"/>
        /// </summary>
        public static Ok<T, E> Ok(T value)
        {
            return new Ok<T, E>(value);
        }

        /// <summary>
        /// Creates a new <see cref="Error{T, E}"/> result containing <paramref name="value"/>
        /// </summary>
        public static Error<T, E> Error(E value)
        {
            return new Error<T, E>(value);
        }

        /// <returns>true if this <see cref="Result{T, E}"/> is <see cref="Ok{T, E}"/></returns>
        public bool IsOk() => this is Ok<T, E>;

        /// <returns>true if this <see cref="Result{T, E}"/> is <see cref="Error{T, E}"/></returns>
        public bool IsError() => this is Error<T, E>;

        /// <summary>
        /// Converts this <see cref="Result{T, E}"/> to an <see cref="Option{T}"/>
        /// </summary>
        /// <returns>an <see cref="Option{T}"/> containing the value of <see cref="Ok{T, E}"/></returns>
        /// <remarks>If this <see cref="Result{T, E}"/> is <see cref="Error{T, E}"/>, the method will return an empty <see cref="Option{T}"/></remarks>
        public Option<T> GetOk()
        {
            return this switch
            {
                Ok<T, E> ok => Option.Some<T>(ok),
                Error<T, E> _ => Option.None<T>()
            };
        }

        /// <summary>
        /// Converts this <see cref="Result{T, E}"/> to an <see cref="Option{T}"/>
        /// </summary>
        /// <returns>an <see cref="Option{T}"/> containing the value of an <see cref="Error{T, E}"/></returns>
        /// <remarks>If this <see cref="Result{T, E}"/> is <see cref="Ok{T, E}"/>, the method will return an empty <see cref="Option{T}"/></remarks>
        public Option<E> GetError()
        {
            return this switch
            {
                Ok<T, E> _ => Option.None<E>(),
                Error<T, E> error => Option.Some<E>(error)
            };
        }

        /// <returns>the contained <see cref="Ok{T, E}"/> value</returns>
        /// <remarks>Throws an <see cref="InvalidOperationException"/> if the <see cref="Result{T, E}"/> is an <see cref="Error{T, E}"/></remarks>
        /// <exception cref="InvalidOperationException"></exception>
        public T Unwrap()
        {
            return this switch
            {
                Ok<T, E> ok => ok,
                Error<T, E> _ => throw new InvalidOperationException("Cannot unwrap an Error Result")
            };
        }

        /// <returns>the contained <see cref="Ok{T, E}"/> value,
        /// or <paramref name="default"/> if the <see cref="Result{T, E}"/> is an <see cref="Error{T, E}"/></returns>
        public T UnwrapOr(T @default)
        {
            return this switch
            {
                Ok<T, E> ok => ok,
                Error<T, E> _ => @default
            };
        }

        /// <returns>the contained <see cref="Ok{T, E}"/> value,
        /// or a value returned by <paramref name="func"/> which consumes the contained error value <typeparamref name="E"/></returns>
        public T UnwrapOrElse(Func<E, T> func)
        {
            return this switch
            {
                Ok<T, E> ok => ok,
                Error<T, E> error => func(error)
            };
        }

        /// <summary>
        /// Applies the function <paramref name="func"/> to the contained <see cref="Ok{T, E}"/> value, ignoring an <see cref="Error{T, E}"/>
        /// </summary>
        /// <typeparam name="U">the new <see cref="Ok{T, E}"/> value type</typeparam>
        public Result<U, E> Map<U>(Func<T, U> func)
        {
            return this switch
            {
                Ok<T, E> ok => Result<U, E>.Ok(func(ok)),
                Error<T, E> error => Result<U, E>.Error(error)
            };
        }

        /// <summary>
        /// Applies the function <paramref name="func"/> to the contained <see cref="Ok{T, E}"/> value or returns <paramref name="default"/>
        /// if this <see cref="Result{T, E}"/> is an <see cref="Error{T, E}"/>
        /// </summary>
        /// <typeparam name="U">value returned by <paramref name="func"/> or <paramref name="default"/></typeparam>
        public U MapOr<U>(U @default, Func<T, U> func)
        {
            return this switch
            {
                Ok<T, E> ok => func(ok),
                Error<T, E> _ => @default
            };
        }

        /// <summary>
        /// Applies the function <paramref name="func"/> to the contained <see cref="Ok{T, E}"/> value, or <paramref name="default"/>
        /// to the contained <see cref="Error{T, E}"/> value
        /// </summary>
        /// <typeparam name="U">value returned by <paramref name="func"/> or <paramref name="default"/></typeparam>
        public U MapOrElse<U>(Func<E, U> @default, Func<T, U> func)
        {
            return this switch
            {
                Ok<T, E> ok => func(ok),
                Error<T, E> error => @default(error)
            };
        }
        
        /// <summary>
        /// Applies the function <paramref name="func"/> to the contained <see cref="Error{T, E}"/> value, ignoring an <see cref="Ok{T, E}"/> result
        /// </summary>
        public Result<T, F> MapError<F>(Func<E, F> func)
        {
            return this switch
            {
                Ok<T, E> ok => Result<T, F>.Ok(ok),
                Error<T, E> error => Result<T, F>.Error(func(error))
            };
        }

        /// <returns><paramref name="result"/> if this <see cref="Result{T, E}"/> is <see cref="Ok{T, E}"/>,
        /// otherwise returns the <see cref="Error{T, E}"/> value of this <see cref="Result{T, E}"/></returns>
        public Result<U, E> And<U>(Result<U, E> result)
        {
            return this switch
            {
                Ok<T, E> _ => result,
                Error<T, E> error => Result<U, E>.Error(error)
            };
        }

        /// <returns>the <see cref="Result{T, E}"/> returned by <paramref name="func"/> if this <see cref="Result{T, E}"/> is <see cref="Ok{T, E}"/>,
        /// otherwise returns the <see cref="Error{T, E}"/> value of this <see cref="Result{T, E}"/></returns>
        public Result<U, E> AndThen<U>(Func<T, Result<U, E>> func)
        {
            return this switch
            {
                Ok<T, E> ok => func(ok),
                Error<T, E> error => Result<U, E>.Error(error)
            };
        }

        /// <returns><paramref name="result"/> if this <see cref="Result{T, E}"/> is an <see cref="Error{T, E}"/>,
        /// otherwise returns the <see cref="Ok{T, E}"/> value of this <see cref="Result{T, E}"/></returns>
        public Result<T, F> Or<F>(Result<T, F> result)
        {
            return this switch
            {
                Ok<T, E> ok => Result<T, F>.Ok(ok),
                Error<T, E> _ => result
            };
        }

        /// <returns>the <see cref="Result{T, E}"/> returned by <paramref name="func"/> if this <see cref="Result{T, E}"/> is an <see cref="Error{T, E}"/>,
        /// otherwise returns the <see cref="Ok{T, E}"/> value of this <see cref="Result{T, E}"/></returns>
        public Result<T, F> OrElse<F>(Func<E, Result<T, F>> func)
        {
            return this switch
            {
                Ok<T, E> ok => Result<T, F>.Ok(ok),
                Error<T, E> error => func(error)
            };
        }

        /// <summary>
        /// Calls <paramref name="func"/> catching any <see cref="Exception"/> it throws into an <see cref="Error{T, E}"/>,
        /// if no <see cref="Exception"/> is thrown, an <see cref="Ok{T, E}"/> value is returned
        /// </summary>
        public static Result<TResult, Exception> Catch<TResult>(Func<TResult> func)
        {
            try
            {
                return Result<TResult, Exception>.Ok(func());
            }
            catch(Exception exception)
            {
                return Result<TResult, Exception>.Error(exception);
            }
        }
    }

    /// <summary>
    /// Represents an Ok <see cref="Result{T, E}"/>
    /// </summary>
    public sealed class Ok<T, E> : Result<T, E>
    {
        /// <summary>
        /// The value contained within this <see cref="Ok{T, E}"/>
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Creates a new <see cref="Ok{T, E}"/> result containing <paramref name="value"/>
        /// </summary>
        public Ok(T value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Returns <see cref="Value"/>
        /// </summary>
        public static implicit operator T(Ok<T, E> result)
        {
            return result.Value;
        }
    }

    /// <summary>
    /// Represents an Error <see cref="Result{T, E}"/>
    /// </summary>
    public sealed class Error<T, E> : Result<T, E>
    {
        /// <summary>
        /// The value contained within this <see cref="Error{T, E}"/>
        /// </summary>
        public E Value { get; private set; }

        /// <summary>
        /// Creates a new <see cref="Error{T, E}"/> result containing <paramref name="value"/>
        /// </summary>
        public Error(E value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Returns <see cref="Value"/>
        /// </summary>
        public static implicit operator E(Error<T, E> result)
        {
            return result.Value;
        }
    }

    /// <summary>
    /// Represents an empty type
    /// </summary>
    public struct Unit
    {
        
    }
}
