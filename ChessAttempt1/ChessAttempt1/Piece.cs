using System;
using System.Collections.Generic;
using System.Text;

namespace ChessAttempt1
{
    class Piece
    {
        //piece name
        public string PieceName { get; private set; }

        //piece symbol
        public char PieceSymbol { get; private set; }

        //determine the color
        public bool IsWhitePiece { get; private set; }

        //possibility for expanding past standard chess armies
        public string Army { get; private set; }

        //has this piece moved?  relevant for pawns, castling
        public bool HasMoved { get;  set; }

        //this one seems to have been rendered redundant, leaving in for now
        //determine if piece is threatening the opposing king on the current board... may nto be used here
        //***currently superfluous, consider removing in future update
        public  bool ThreatensKing { get; private set; }

        //piece rank, if used.  pawn = 1, bishop/knight = 3, rook = 5, queen = 9, king = 10000
        public int PieceRank { get; private set; }

        //fills in the information for a piece
        public void AddPiece(string piece, string armyName, bool isWhite)
        {
            Army = armyName;
            ThreatensKing = false;
            IsWhitePiece = isWhite;
            PieceName = piece;
            HasMoved = false;
            //unicode does NOT look good in console window, may update later
            if (PieceName == "pawn" && IsWhitePiece)
            {
                //pieceSymbol = '\u265f';
                //instead of the actual unicode, just copy pasting the symbols here
                PieceSymbol = '♟';
                PieceRank = 1;
            }
            else if (PieceName == "pawn" && IsWhitePiece == false)
            {
                PieceSymbol = '♙';
                PieceRank = 1;
            }
            else if (PieceName == "knight" && IsWhitePiece)
            {
                PieceSymbol = '♞';
                PieceRank = 3;
            }
            else if (PieceName == "knight" && IsWhitePiece == false)
            {
                PieceSymbol = '♘';
                PieceRank = 3;
            }
            else if (PieceName == "bishop" && IsWhitePiece)
            {
                PieceSymbol = '♝';
                PieceRank = 3;
            }
            else if (PieceName == "bishop" && IsWhitePiece == false)
            {
                PieceSymbol = '♗';
                PieceRank = 3;
            }
            else if (PieceName == "rook" && IsWhitePiece)
            {
                PieceSymbol = '♜';
                PieceRank = 5;
            }
            else if (PieceName == "rook" && IsWhitePiece == false)
            {
                PieceSymbol = '♖';
                PieceRank = 5;
            }
            else if (PieceName == "queen" && IsWhitePiece)
            {
                PieceSymbol = '♛';
                PieceRank = 9;
            }
            else if (PieceName == "queen" && IsWhitePiece == false)
            {
                PieceSymbol = '♕';
                PieceRank = 9;
            }
            else if (PieceName == "king" && IsWhitePiece)
            {
                PieceSymbol = '♚';
                PieceRank = 10000;
            }
            else if (PieceName == "king" && IsWhitePiece == false)
            {
                PieceSymbol = '♔';
                PieceRank = 10000;
            }
        }
    }
}
