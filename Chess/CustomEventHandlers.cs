namespace Chess
{
    delegate void GameStartEventHandler(object sender, GameStartEventArgs e);
    delegate void MoveEventHandler(object sender, MoveEventArgs e);
    delegate void LegalMovesEventHandler(object sender, LegalMovesEventArgs e);
    delegate void MultipleOptionEventHandler(object sender, MultipleOptionEventArgs e);
}
