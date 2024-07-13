namespace PaySkyTask.Core.Result;

public class ValidationError
{
    public string ErrorMessage;

    public ValidationError()
    {
    }

    public ValidationError(string errorMessage)
    {
        Errors = errorMessage;
    }

    public ValidationError(string identifier, string errorMessage, string errorCode, ValidationSeverity severity)
    {
        Identifier = identifier;
        Errors = errorMessage;
        ErrorCode = errorCode;
        Severity = severity;
    }

    public string Identifier { get; set; }
    public string Errors { get; set; }
    public string ErrorCode { get; set; }
    public ValidationSeverity Severity { get; set; } = ValidationSeverity.Error;
}
