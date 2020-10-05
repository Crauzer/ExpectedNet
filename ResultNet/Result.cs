using System;

// Disable error because we do not allow users to derive from Result
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).

namespace ResultNet
{
    public abstract class Result<T, E>
    {
        // Internal constructor to prevent users from deriving from this type
        internal Result()
        {

        }

        public static Ok<T, E> Ok(T value)
        {
            return new Ok<T, E>(value);
        }
        public static Error<T, E> Error(E error)
        {
            return new Error<T, E>(error);
        }

        public bool IsOk() => this is Ok<T, E>;
        public bool IsError() => this is Error<T, E>;

        public T Unwrap()
        {
            return this switch
            {
                Ok<T, E> ok => ok,
                Error<T, E> _ => throw new InvalidOperationException("Cannot unwrap an Error Result"),
                _ => throw new InvalidOperationException("Unknown Result type"),
            };
        }

        public Result<U, E> Map<U>(Func<T, U> func)
        {
            return this switch
            {
                Ok<T, E> ok => Result<U, E>.Ok(func(ok)),
                Error<T, E> error => Result<U, E>.Error(error)
            };
        }
        public U MapOr<U>(U @default, Func<T, U> func)
        {
            return this switch
            {
                Ok<T, E> ok => func(ok),
                Error<T, E> _ => @default
            };
        }
        public U MapOrElse<U>(Func<E, U> @default, Func<T, U> func)
        {
            return this switch
            {
                Ok<T, E> ok => func(ok),
                Error<T, E> error => @default(error)
            };
        }
        public Result<T, F> MapError<F>(Func<E, F> func)
        {
            return this switch
            {
                Ok<T, E> ok => Result<T, F>.Ok(ok),
                Error<T, E> error => Result<T, F>.Error(func(error))
            };
        }
    
        public Result<U, E> And<U>(Result<U, E> result)
        {
            return this switch
            {
                Ok<T, E> _ => result,
                Error<T, E> error => Result<U, E>.Error(error)
            };
        }
        public Result<U, E> AndThen<U>(Func<T, Result<U, E>> func)
        {
            return this switch
            {
                Ok<T, E> ok => func(ok),
                Error<T, E> error => Result<U, E>.Error(error)
            };
        }
        
        public Result<T, F> Or<F>(Result<T, F> result)
        {
            return this switch
            {
                Ok<T, E> ok => Result<T, F>.Ok(ok),
                Error<T, E> _ => result
            };
        }

    }

    public sealed class Ok<T, E> : Result<T, E>
    {
        private T _value;

        public Ok(T value)
        {
            this._value = value;
        }

        public T Get() => this._value;

        public static implicit operator T(Ok<T, E> result)
        {
            return result._value;
        }
    }

    public sealed class Error<T, E> : Result<T, E>
    {
        private E _error;

        public Error(E error)
        {
            this._error = error;
        }

        public E Get() => this._error;

        public static implicit operator E(Error<T, E> result)
        {
            return result._error;
        }
    }
}
