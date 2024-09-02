using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Errors;

namespace Domain.Results
{
    public class Result<T>
    {
        public T? Value {get; init;}
        public Error? Error {get; init;}
        public bool IsSuccess {get; init;}
        
        private Result(T? value, Error? error, bool isSuccess)
        {
            Value = value;
            Error = error;
            IsSuccess = isSuccess;
        }

        public static Result<T> Success(T value) => new(value, null, true);
        public static Result<T> Failure(Error error) => new(default, error, false);
        
    }
}