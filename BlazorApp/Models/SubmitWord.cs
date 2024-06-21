namespace BlazorApp.Models
{
    public record SubmitWord(string User, string Word)
    {
        public override string ToString()
        {
            return $"{User}: {Word}";
        }
    }
}