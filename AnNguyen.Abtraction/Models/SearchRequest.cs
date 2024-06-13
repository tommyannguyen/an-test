namespace AnNguyen.Abtraction.Models;

public record SearchRequest(string Query, string Match, int Limit = 100);
