using System;
using System.Collections.Generic;
using System.Text;

namespace GuessNumberGame.Domain.Utils.SettingsUtils
{
    public sealed class Result
    {
        public bool IsSuccess { get; }
        public IReadOnlyList<string> Errors { get; }

        private Result(bool isSuccess, IReadOnlyList<string> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors ?? Array.Empty<string>();
        }

        public static Result Ok() => new(true, Array.Empty<string>());

        public static Result Fail(params string[] errors) =>
            new(false, errors.Length == 0 ? Array.Empty<string>() : errors);

        public static Result Fail(IEnumerable<string> errors) =>
            new(false, errors?.ToArray() ?? Array.Empty<string>());

        public static implicit operator bool(Result result) => result.IsSuccess;
    }
}
