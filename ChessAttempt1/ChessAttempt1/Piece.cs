using System;
using System.Collections.Generic;
using System.Text;

namespace ChessAttempt1
{
    class Piece
    {
        //piece name
        public string pieceName { get; private set; }

        //piece symbol
        public char pieceSymbol { get; private set; }

        //determine the color
        public bool isWhitePiece { get; private set; }

        //possibility for expanding past standard chess armies
        public string army { get; private set; }

        //has this piece moved?  relevant for pawns, castling
        public bool hasMoved { get;  set; }

        //determine if piece is threatening the opposing king on the current board... may nto be used here
        public  bool threatensKing { get; private set; }

        //piece rank, if used.  pawn = 1, bishop/knight = 3, rook = 5, queen = 9, king = 200
        public int pieceRank { get; private set; }

        public void addPiece(string piece, string armyName, bool isWhite)
        {
            army = armyName;
            threatensKing = false;
            isWhitePiece = isWhite;
            pieceName = piece;
            hasMoved = false;
            //unicode does NOT look good in console window, will update later
            if (pieceName == "pawn" && isWhitePiece)
            {
                //pieceSymbol = '\u265f';
                pieceSymbol = '♟';
                pieceRank = 1;
            }
            else if (pieceName == "pawn" && isWhitePiece == false)
            {
                pieceSymbol = '♙';
                pieceRank = 1;
            }
            else if (pieceName == "knight" && isWhitePiece)
            {
                pieceSymbol = '♞';
                pieceRank = 3;
            }
            else if (pieceName == "knight" && isWhitePiece == false)
            {
                pieceSymbol = '♘';
                pieceRank = 3;
            }
            else if (pieceName == "bishop" && isWhitePiece)
            {
                pieceSymbol = '♝';
                pieceRank = 3;
            }
            else if (pieceName == "bishop" && isWhitePiece == false)
            {
                pieceSymbol = '♗';
                pieceRank = 3;
            }
            else if (pieceName == "rook" && isWhitePiece)
            {
                pieceSymbol = '♜';
                pieceRank = 5;
            }
            else if (pieceName == "rook" && isWhitePiece == false)
            {
                pieceSymbol = '♖';
                pieceRank = 5;
            }
            else if (pieceName == "queen" && isWhitePiece)
            {
                pieceSymbol = '♛';
                pieceRank = 9;
            }
            else if (pieceName == "queen" && isWhitePiece == false)
            {
                pieceSymbol = '♕';
                pieceRank = 9;
            }
            else if (pieceName == "king" && isWhitePiece)
            {
                pieceSymbol = '♚';
                pieceRank = 200;
            }
            else if (pieceName == "king" && isWhitePiece == false)
            {
                pieceSymbol = '♔';
                pieceRank = 200;
            }
        }
    }
}
