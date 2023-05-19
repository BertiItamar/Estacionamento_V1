using System.Globalization;

namespace Infrastructure.DTO;

public class AppException : Exception
{
    public AppException() : base() { }
    public string Message { get; set; }

    public AppException(string message) : base(message) { }

    public AppException(string message, params object[] args)
        : base(String.Format(CultureInfo.CurrentCulture, message, args)) { }
}


