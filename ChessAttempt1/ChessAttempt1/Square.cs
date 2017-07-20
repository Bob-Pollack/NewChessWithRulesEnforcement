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

        //color of square, if necessary
        public bool isWhiteSquare { get; private set; }

        //determine if the square is empty
        public bool hasPiece { get; set; }

        //piece on it if any
        public Piece occupyingPiece { get; set; }

        public void setRow(int rowNumber)
        {
            row = rowNumber;
            rank = (9 - row).ToString();
        }

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

        public void setSquareColor(bool isItWhite)
        {
            isWhiteSquare = isItWhite;
        }

        public void setSquareOccupied(bool isOccupied)
        {
            hasPiece = isOccupied;
        }

        public void SetSquarePiece(Piece newPiece)
        {
            occupyingPiece = newPiece;
        }
    }
}
