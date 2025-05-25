namespace DataAggregator.Registration.Exceptions;

/// <summary>
/// Exception thrown when there is an issue accessing the database.
/// </summary>
public class DatabaseAccessException(string message, Exception? innerException = null)
    : Exception(message, innerException)
{
}
