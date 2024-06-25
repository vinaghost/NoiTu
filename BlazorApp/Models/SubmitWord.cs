namespace BlazorApp.Models
{
    public record SubmitWord(string User, string Word, List<Definition> Definitions);
}