using System;
using System.Collections.Generic;
using System.Text;

namespace ChessAttempt1
{
    class Board
    {
        //probably want to do smomething more complicated here but we'll see
        public List<Square> BoardSquares { get; private set; }
        public bool isWhiteTurn { get; set; }
        public List<Piece> capturedWhitePieces { get; set; }
        public List<Piece> capturedBlackPieces { get; set; }
        public bool isGameOver { get; set; }
        public string winMessage { get; set;}
        public bool whiteInCheck { get; set; }
        public bool blackInCheck { get; set; }
        //probably need to store the last move here for the sake of en passant rules 
        //may be better to have it be a full list of moves to be used as a log
        //not entirely sure how to do the repeat position check but we'll get there later


        public Board()
        {
            BoardSquares = new List<Square>();
            isWhiteTurn = true;
            capturedBlackPieces = new List<Piece>();
            capturedWhitePieces = new List<Piece>();
            isGameOver = false;
            winMessage = "Test code this should never show up";
            whiteInCheck = false;
            blackInCheck = false;
        }

        internal void fillBoard(int whiteArmy, int blackArmy)
        {
            //board is filled with squares, some of which have pieces on them.
            //list of squares starts from upper left corner of the board (8A by rank and file system) as 0
            // and bottom right as corner (1H by rank and file system) as 63.
            //bottom left corner 1A is 56, top right corner 8F is 7
            string whiteArmyName = "";
            string blackArmyName = "";

            //add elses to these if I ever do more than just classic
            if (whiteArmy == 1)
            {
                whiteArmyName = "classic";
            }
            if (blackArmy == 1)
            {
                blackArmyName = "classic";
            }


            //black back row
            for (int i = 1; i <= 8; i++)
            {
                bool isSquareWhite = (i % 2 > 0);
                bool isWhitePiece = false;
                string piece = "";
                if (i == 1 || i == 8)
                {
                    piece = "rook";
                }
                else if (i == 2 || i == 7)
                {
                    piece = "knight";
                }
                else if (i == 3 || i == 6)
                {
                    piece = "bishop";
                }
                else if (i == 4)
                {
                    piece = "queen";
                }
                else if (i == 5)
                {
                    piece = "king";
                }

                Piece p = new Piece();
                p.addPiece(piece, blackArmyName, isWhitePiece);

                Square s = new Square();
                s.setRow(1);
                s.setColumn(i);
                s.setSquareColor(isSquareWhite);
                s.setSquareOccupied(true);
                s.SetSquarePiece(p);
                BoardSquares.Add(s);
            }
            //black pawn row
            for(int i = 1; i <= 8; i++)
            {
                bool isSquareWhite = (i % 2 == 0);
                bool isWhitePiece = false;
                string piece = "pawn";


                Piece p = new Piece();
                p.addPiece(piece, blackArmyName, isWhitePiece);

                Square s = new Square();
                s.setRow(2);
                s.setColumn(i);
                s.setSquareColor(isSquareWhite);
                s.setSquareOccupied(true);
                s.SetSquarePiece(p);
                BoardSquares.Add(s);
            }
            //four empty rows
            for (int j = 3; j <= 6; j++)
            {
                for (int i = 1; i <= 8; i++)
                {
                    bool isSquareWhite = ((i + j) % 2 == 0);
                    Square s = new Square();
                    s.setRow(j);
                    s.setColumn(i);
                    s.setSquareColor(isSquareWhite);
                    s.setSquareOccupied(false);
                    BoardSquares.Add(s);                   
                }
            }
            //white pawns
            for (int i = 1; i <= 8; i++)
            {
                bool isSquareWhite = (i % 2 > 0);
                bool isWhitePiece = true;
                string piece = "pawn";


                Piece p = new Piece();
                p.addPiece(piece, whiteArmyName, isWhitePiece);

                Square s = new Square();
                s.setRow(7);
                s.setColumn(i);
                s.setSquareColor(isSquareWhite);
                s.setSquareOccupied(true);
                s.SetSquarePiece(p);
                BoardSquares.Add(s);
            }
            //white back row
            for (int i = 1; i <= 8; i++)
            {
                bool isSquareWhite = (i % 2 == 0);
                bool isWhitePiece = true;
                string piece = "";
                if (i == 1 || i == 8)
                {
                    piece = "rook";
                }
                else if (i == 2 || i == 7)
                {
                    piece = "knight";
                }
                else if (i == 3 || i == 6)
                {
                    piece = "bishop";
                }
                else if (i == 4)
                {
                    piece = "queen";
                }
                else if (i == 5)
                {
                    piece = "king";
                }

                Piece p = new Piece();
                p.addPiece(piece, whiteArmyName, isWhitePiece);

                Square s = new Square();
                s.setRow(8);
                s.setColumn(i);
                s.setSquareColor(isSquareWhite);
                s.setSquareOccupied(true);
                s.SetSquarePiece(p);
                BoardSquares.Add(s);
            }

        }
    }
}
