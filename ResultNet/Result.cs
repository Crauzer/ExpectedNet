using System;

namespace ResultNet
{
    public abstract class Result<T, E>
    {
        public static Ok<T, E> Ok(T value)
        {
            return new Ok<T, E>(value);
        }
        public static Error<T, E> Error(E error)
        {
            return new Error<T, E>(error);
        }

        public bool IsOk() => this is Ok<T, E>;
        public bool IsError() => this is Error<E, T>;

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
                Error<T, E> error => Result<U, E>.Error(error),
                _ => throw new InvalidOperationException("Unknown Result type"),
            };
        }
        public Result<U, E> MapOr<U>(U @default, Func<T, U> func)
        {
            return this switch
            {
                Ok<T, E> ok => Result<U, E>.Ok(func(ok)),
                Error<T, E> error => Result<U, E>.Ok(@default),
                _ => throw new InvalidOperationException("Unknown Result type"),
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

        public static implicit operator E(Error<T, E> result)
        {
            return result._error;
        }
    }
}
