using Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    //public readonly struct Result<TValue>
    //{
    //    public readonly TValue? Value;
    //    public readonly Error? Error;

    //    private Result(TValue value)
    //    {
    //        IsError = false;
    //        Value = value;
    //        Error = default;
    //    }

    //    private Result(Error error)
    //    {
    //        IsError = true;
    //        Error = error;
    //        Value = default;
    //    }

    //    public bool IsError { get; }
    //    public bool IsSuccess => !IsError;

    //    public static implicit operator Result<TValue>(TValue value) => new(value);

    //    public static implicit operator Result<TValue>(Error error) => new(error);

    //    public TResult Match<TResult>(
    //        Func<TValue, TResult> success,
    //        Func<Error, TResult> failure) =>
    //        !IsError ? success(Value!) : failure(Error!);
    //}

    //public readonly struct Result<TValue, TError>
    //{
    //    public readonly TValue? Value;
    //    public readonly Error<TError>? Error;

    //    private Result(TValue value)
    //    {
    //        IsError = false;
    //        Value = value;
    //        Error = default;
    //    }

    //    private Result(Error<TError> error)
    //    {
    //        IsError = true;
    //        Error = error;
    //        Value = default;
    //    }

    //    public bool IsError { get; }
    //    public bool IsSuccess => !IsError;

    //    public static implicit operator Result<TValue, TError>(TValue value) => new(value);

    //    public static implicit operator Result<TValue, TError>(Error<TError> error) => new(error);

    //    public TResult Match<TResult>(
    //        Func<TValue, TResult> success,
    //        Func<Error<TError>, TResult> failure) =>
    //        !IsError ? success(Value!) : failure(Error!);
    //}
}
