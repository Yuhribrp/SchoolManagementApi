using System.Diagnostics.Contracts;

namespace SchoolManagementApi.Utils {
    public class Result {
        public bool Success { get; private set; }

        public string Error { get; private set; }

        public bool Failure {
            get { return !Success; }
        }

        protected Result(bool success, string error) {
            Contract.Requires(success || !string.IsNullOrEmpty(error));
            Contract.Requires(!success || string.IsNullOrEmpty(error));

            Success = success;
            Error = error;
        }

        public static Result Fail(string message) {
            return new Result(false, message);
        }

        public static Result<T> Fail<T>(string message) {
            return new Result<T>(default(T), false, message);
        }

        public static Result Ok() {
            return new Result(true, String.Empty);
        }

        public static Result<T> Ok<T>(T value) {
            return new Result<T>(value, true, String.Empty);
        }

        public static Result Combine(params Result[] results) {
            foreach (Result result in results) {
                if (result.Failure)
                    return result;
            }

            return Ok();
        }
    }


    public class Result<T> : Result {
        private T _value;

        public T Value {
            get {
                Contract.Requires(Success);

                return _value;
            }
            //[param: AllowNull]
            private set { _value = value; }
        }

        internal protected Result(/*[AllowNull]*/ T value, bool success, string error = null)
            : base(success, error) {
            Contract.Requires(value != null || !success);

            Value = value;
        }
    }
}
