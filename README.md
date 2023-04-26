# Multimode Chess

This repository contains a Windows chess game implementation with support for different game modes and a generic Minimax AI.

Currently supported modes:

* Standard
* Pawn of the Dead (inspired by a game available [here](https://store.steampowered.com/app/890130/Pawn_of_the_Dead/))

## Structure

The repository contains a single Visual Studio solution consisting of two projects:

* ChessCore - implements the chess logic (game modes, pieces, moves, AI)
* ChessGUI - implements a WinForms GUI client
