namespace UniversalSolution.Middleware.Exceptions;

public abstract class CustomException : Exception
{
    public int StatusCode { get; }

    protected CustomException(string message, int statusCode) 
        : base(message)
    {
        StatusCode = statusCode;
    }
}

public class ExternalApiException : CustomException
{
    public ExternalApiException(string message) : base(message, 502) { }
}

public class NotFoundException : CustomException
{
    public NotFoundException(string message) : base(message, 404) { }
}

public class ConflictException : CustomException
{
    public ConflictException(string message) : base(message, 409) { }
}

public class UnauthorizedException : CustomException
{
    public UnauthorizedException(string message) : base(message, 401) { }
}