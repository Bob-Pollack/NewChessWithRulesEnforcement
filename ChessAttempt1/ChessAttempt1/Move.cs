using System;
using System.Collections.Generic;
using System.Text;

namespace ChessAttempt1
{
    class Move
    {
        //this class is a single move on the chess board.  it should track 
            //the board square numbers, 
            //rank and file of start and end points, 
            //the piece that made the move
            //if a piece was captured
            //what piece, if any, was captured
        //this should have all of the information necessary to log the moves and undo a move.

        public int StartingSquare { get; set; }
        public int TargetSquare { get; set; }
        public Piece MovingPiece { get; set; }
        public bool WasTargetSquareEmpty { get; set; }
        public Piece CapturedPiece { get; set; }
        public string StartingRank { get; set; }
        public string StartingFile { get; set; }
        public string TargetRank { get; set; }
        public string TargetFile { get; set; }
        public bool HasThisPieceMovedBefore { get; set; }
        public bool Check { get; set; }
    }
}
