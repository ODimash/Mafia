using FluentResults;

namespace Mafia.Shared.API.Models;

public class ResponseModel
{
	public bool IsSuccess { get; set; }
	public List<string>? Errors { get; set; }

	public static ResponseModel Ok() => new() { IsSuccess = true };

	public static ResponseModel Fail(List<string> errors) => new() { IsSuccess = false, Errors = errors };
}
public class ResponseModel<T>
{
		public bool IsSuccess { get; set; }
		public T? Data { get; set; }
		public List<string>? Errors { get; set; }

		public static ResponseModel<T> Ok(T data) => new() { IsSuccess = true, Data = data };

		public static ResponseModel<T> Fail(List<string> errors) => new() { IsSuccess = false, Errors = errors };
}

public static class ResultExtensions
{
	public static ResponseModel<T> ToResponse<T>(this Result<T> result)
	{
		return new ResponseModel<T>
		{
			IsSuccess = result.IsSuccess,
			Data = result.IsSuccess ? result.ValueOrDefault : default,
			Errors = result.IsFailed 
				? result.Errors.Select(e => e.Message).ToList()
				: null,
		};
	}
	
	public static ResponseModel<TResponse> ToResponse<TResult, TResponse>(this Result<TResult> result, Func<TResult, TResponse> responseFunc)
	{
		return new ResponseModel<TResponse>
		{
			IsSuccess = result.IsSuccess,
			Data = result.IsSuccess ? responseFunc.Invoke(result.Value) : default,
			Errors = result.IsFailed 
				? result.Errors.Select(e => e.Message).ToList()
				: null,
		};
	}
	
	public static ResponseModel ToEmptyResponse<T>(this Result<T> result)
	{
		return new ResponseModel
		{
			IsSuccess = result.IsSuccess,
			Errors = result.IsFailed 
				? result.Errors.Select(e => e.Message).ToList()
				: null,
		};
	}

	public static ResponseModel ToResponse(this Result result)
	{
		return new ResponseModel
		{
			IsSuccess = result.IsSuccess,
			Errors = result.IsFailed 
				? result.Errors.Select(e => e.Message).ToList()
				: null,
		};
	}
}