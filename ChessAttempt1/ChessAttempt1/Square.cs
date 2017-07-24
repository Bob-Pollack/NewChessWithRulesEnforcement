using System;
using System.Collections.Generic;
using System.Text;

namespace ChessAttempt1
{
    class Square
    {
        //determine location on the board
        public int row { get; private set; }
        public int column { get; private set; }
        public string rank;
        public string file;

        //color of square
        public bool isWhiteSquare { get; private set; }

        //determine if the square is empty
        public bool hasPiece { get; set; }

        //piece on it if any
        public Piece occupyingPiece { get; set; }

        //stores the row and rank of the square based on a rownumber input
        public void setRow(int rowNumber)
        {
            row = rowNumber;
            rank = (9 - row).ToString();
        }

        //stores the column and file based on a column number input
        public void setColumn(int columnNumber)
        {
            column = columnNumber;
            switch (column)
            {
                case 1:
                    file = "a";
                    break;
                case 2:
                    file = "b";
                    break;
                case 3:
                    file = "c";
                    break;
                case 4 :
                    file = "d";
                    break;
                case 5:
                    file = "e";
                    break;
                case 6:
                    file = "f";
                    break;
                case 7:
                    file = "g";
                    break;
                case 8:
                    file = "h";
                    break;
            }

        }

        //sets the square color bool
        public void setSquareColor(bool isItWhite)
        {
            isWhiteSquare = isItWhite;
        }

        //sets the bool for if the square has a piece on it
        //if this is not set, square may still have a piece but it will be ignored
        public void setSquareOccupied(bool isOccupied)
        {
            hasPiece = isOccupied;
        }

        //set a new piece on the square
        public void SetSquarePiece(Piece newPiece)
        {
            occupyingPiece = newPiece;
        }
    }
}
