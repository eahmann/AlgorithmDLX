namespace AlgorithmDLX.UnitTest.Classes.Exceptions;

internal class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }

    public ValidationException(string message, Exception innerException) : base(message, innerException) { }
}