using System;

namespace ResultNet
{
    public abstract class Result<T, E>
    {
        public static Ok<T> Ok(T value)
        {
            return new Ok<T>(value);
        }
        public static Error<E> Error(E error)
        {
            return new Error<E>(error);
        }

        public bool IsOk() => this is Ok<T>;
        public bool IsError() => this is Error<E>;

        public T Unwrap()
        {
            switch (this)
            {
                case Ok<T> ok: return ok;
                case Error<T> error: throw new InvalidOperationException("Cannot unwrap an Error Result");
                default: throw new InvalidOperationException("Unknown Result type");
            }
        }
    }

    public sealed class Ok<T> : Result<T, int>
    {
        private T _value;

        public Ok(T value)
        {
            this._value = value;
        }

        public static implicit operator T(Ok<T> result)
        {
            return result._value;
        }
    }

    public sealed class Error<T> : Result<int, T>
    {
        private T _error;

        public Error(T error)
        {
            this._error = error;
        }

        public static implicit operator T(Error<T> result)
        {
            return result._error;
        }
    }
}
