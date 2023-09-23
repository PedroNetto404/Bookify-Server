using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Utility.Results;

public class Result<TValue> : Result
{
    protected internal Result(TValue value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    private readonly TValue _value;

    public static implicit operator Result<TValue>(TValue value) => Create(value);

    public static implicit operator Result<TValue>(Error error) => Failure<TValue>(error);
}