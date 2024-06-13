using AnNguyen.Abtraction.Models;

namespace AnNguyen.Services.SearchEngine.Google;

public class GoogleErrorExeption(string message) : SearchExeption(message)
{
}
