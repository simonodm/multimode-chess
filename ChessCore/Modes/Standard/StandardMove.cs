namespace ChessCore.Game.Modes.Standard
{
    public abstract class StandardMove : Move
    {
        public StandardMove() : base(GameModePool.Get<StandardRules>()) { }

        public abstract StandardBoardState Process();
    }
}
