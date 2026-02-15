namespace MovieAppRamos.Application.Common.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string entityName, string key)
        : base($"{entityName} with key '{key}' was not found.")
    {
    }
}

