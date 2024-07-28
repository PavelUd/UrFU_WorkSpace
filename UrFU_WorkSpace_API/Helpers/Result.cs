using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Helpers;

public class None
{
    private None()
    {
    }
}

public struct Result<T>
{
    public Result(Error error, T value = default)
    {
        Error = error;
        Value = value;
    }

    public static implicit operator Result<T>(T v)
    {
        return Result.Ok(v);
    }

    public Error Error { get; }
    internal T Value { get; }

    public T GetValueOrThrow()
    {
        if (IsSuccess) return Value;
        throw new InvalidOperationException($"No value. Only Error {Error}");
    }

    public bool IsSuccess => Error == null;
}

public static class Result
{
    public static Result<T> AsResult<T>(this T value)
    {
        return Ok(value);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(null, value);
    }

    public static Result<None> Ok()
    {
        return Ok<None>(null);
    }

    public static Result<T> Fail<T>(Error e)
    {
        return new Result<T>(e);
    }

    public static Result<T> Of<T>(Func<T> f, Error? error = null)
    {
        try
        {
            return Ok(f());
        }
        catch (Exception e)
        {
            return Fail<T>(error ?? new Error(e.Message));
        }
    }

    public static Result<T> Of<T>(Func<Result<T>> f)
    {
        return f();
    }


    public static Result<None> OfAction(Action f, Error? error = null)
    {
        try
        {
            f();
            return Ok();
        }
        catch (Exception e)
        {
            return Fail<None>(error ?? new Error(e.Message));
        }
    }

    public static Result<TOutput> Then<TInput, TOutput>(
        this Result<TInput> input,
        Func<TInput, TOutput> continuation)
    {
        return input.Then(inp => Of(() => continuation(inp)));
    }

    public static Result<None> Then<TInput, TOutput>(
        this Result<TInput> input,
        Action<TInput> continuation)
    {
        return input.Then(inp => OfAction(() => continuation(inp)));
    }

    public static Result<None> Then<TInput>(
        this Result<TInput> input,
        Action<TInput> continuation)
    {
        return input.Then(inp => OfAction(() => continuation(inp)));
    }

    public static Result<TOutput> Then<TInput, TOutput>(
        this Result<TInput> input,
        Func<TInput, Result<TOutput>> continuation)
    {
        return input.IsSuccess
            ? continuation(input.Value)
            : Fail<TOutput>(input.Error);
    }

    public static Result<None> Then<TInput>(
        this Result<None> input, TInput param,
        Action<TInput> continuation)
    {
        return input.Then(inp => OfAction(() => continuation(param)).Value);
    }

    public static Result<TInput> OnFail<TInput>(
        this Result<TInput> input,
        Action<Error> handleError)
    {
        if (!input.IsSuccess) handleError(input.Error);
        return input;
    }

    public static Result<TInput> ReplaceError<TInput>(
        this Result<TInput> input,
        Func<Error, Error> replaceError)
    {
        if (input.IsSuccess) return input;
        return Fail<TInput>(replaceError(input.Error));
    }

    public static Result<TInput> RefineError<TInput>(
        this Result<TInput> input,
        Error error)
    {
        if (input.Error == null) input.ReplaceError(e => error);
        input.Error.SubError = error;
        return input;
    }
}