namespace ChessCore
{
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
