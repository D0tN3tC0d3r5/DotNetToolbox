﻿namespace DotNetToolbox.Results;

public class ValidationException
    : Exception {
    public const string DefaultMessage = "Validation failed.";

    public ValidationError[] Errors { get; init; } = [];

    public ValidationException()
        : base(DefaultMessage) {
        Errors = [new(ValidationError.DefaultErrorMessage)];
    }

    public ValidationException(string message)
        : base(message) {
        Errors = [new(ValidationError.DefaultErrorMessage)];
    }

    public ValidationException(Exception innerException)
        : this(DefaultMessage, innerException) {
    }

    public ValidationException(ValidationError error, Exception? innerException = null)
        : this(DefaultMessage, error, innerException) {
    }

    public ValidationException(IEnumerable<ValidationError> errors, Exception? innerException = null)
        : this(DefaultMessage, errors, innerException) {
    }

    public ValidationException(string message, Exception? innerException = null)
        : this(message, string.Empty, innerException) {
    }

    public ValidationException(string message, ValidationError error, Exception? innerException = null)
        : this(message, string.Empty, error, innerException) {
    }

    public ValidationException(string message, IEnumerable<ValidationError> errors, Exception? innerException = null)
        : this(message, string.Empty, errors, innerException) {
    }

    public ValidationException(string message, string source, Exception? innerException = null)
        : this(message, source, ValidationError.DefaultErrorMessage, innerException) {
    }

    public ValidationException(string message, string source, ValidationError error, Exception? innerException = null)
        : this(message, source, [error], innerException) {
    }

    public ValidationException(string message, string source, IEnumerable<ValidationError> errors, Exception? innerException = null)
        : base(IsNotNullOrWhiteSpace(message), innerException) {
        Errors = errors.Distinct().ToArray();
        Source = source;
    }
}
