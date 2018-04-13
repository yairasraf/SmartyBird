using System;

public class DataDimensionsMismatchException : Exception
{
    public DataDimensionsMismatchException()
    {
    }

    public DataDimensionsMismatchException(string message) : base(message)
    {
    }

    public DataDimensionsMismatchException(string message, Exception inner) : base(message, inner)
    {
    }
}