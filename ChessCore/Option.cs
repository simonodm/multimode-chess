namespace ChessCore
{
    /// <summary>
    /// Represents a single option with an Id and a name.
    /// </summary>
    public class Option
    {
        public int Id { get; }
        public string Text { get; }

        public Option(int id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}
