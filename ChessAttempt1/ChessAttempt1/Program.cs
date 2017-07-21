using System;

namespace ChessAttempt1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("CHESS ATTEMPT 1");

            Board b1 = new Board();

            b1.fillBoard(1, 1);

            DrawBoard(b1);
            while (b1.isGameOver == false)
            {
                TakeTurn(b1);
            }

            Console.WriteLine(b1.winMessage);
            Console.ReadLine();

        }

        private static void TakeTurn(Board inputBoard)
        {
            bool turnInProgress = true;
            while (turnInProgress)
            {


                //attempting to mess with movement of pieces for first time
                bool goodInput = false;
                int inputRowNumber = 0;
                int outputRowNumber = 0;
                int inputColumnNumber = 0;
                int outputColumnNumber = 0;
                while (goodInput == false)
                {


                    Console.WriteLine("select piece to move");
                    Console.WriteLine("Row:");
                    string inputRow = Console.ReadLine().ToLower();
                    Int32.TryParse(inputRow, out inputRowNumber);

                    Console.WriteLine("Column:");
                    string inputColumn = Console.ReadLine().ToLower();
                    switch (inputColumn)
                    {
                        case "a":
                            inputColumnNumber = 1;
                            break;
                        case "b":
                            inputColumnNumber = 2;
                            break;
                        case "c":
                            inputColumnNumber = 3;
                            break;
                        case "d":
                            inputColumnNumber = 4;
                            break;
                        case "e":
                            inputColumnNumber = 5;
                            break;
                        case "f":
                            inputColumnNumber = 6;
                            break;
                        case "g":
                            inputColumnNumber = 7;
                            break;
                        case "h":
                            inputColumnNumber = 8;
                            break;
                        default:
                            inputColumnNumber = 0;
                            break;
                    }


                    Console.WriteLine("move piece to what square");
                    Console.WriteLine("Row:");
                    string outputRow = Console.ReadLine().ToLower();
                    Int32.TryParse(outputRow, out outputRowNumber);

                    Console.WriteLine("Column:");
                    string outputColumn = Console.ReadLine().ToLower();
                    switch (outputColumn)
                    {
                        case "a":
                            outputColumnNumber = 1;
                            break;
                        case "b":
                            outputColumnNumber = 2;
                            break;
                        case "c":
                            outputColumnNumber = 3;
                            break;
                        case "d":
                            outputColumnNumber = 4;
                            break;
                        case "e":
                            outputColumnNumber = 5;
                            break;
                        case "f":
                            outputColumnNumber = 6;
                            break;
                        case "g":
                            outputColumnNumber = 7;
                            break;
                        case "h":
                            outputColumnNumber = 8;
                            break;
                        default:
                            outputColumnNumber = 0;
                            break;
                    }


                    if (inputColumnNumber != 0 && outputColumnNumber != 0
                        && inputRowNumber != 0 && outputRowNumber != 0)
                    {
                        goodInput = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Bad input, try again");
                        DrawBoard(inputBoard);
                    }
                }

                int inputSquareNumber = 0;
                //get the start of the row in the board that was moved
                inputSquareNumber = 8 * (8 - inputRowNumber) + inputColumnNumber - 1;


                int outputSquareNumber = 0;
                outputSquareNumber = 8 * (8 - outputRowNumber) + outputColumnNumber - 1;

                //test code
                //Console.WriteLine("moving from " + inputSquareNumber + "to " + outputSquareNumber);
                //Console.ReadLine();

                Square s1 = inputBoard.BoardSquares[inputSquareNumber];
                Square s2 = inputBoard.BoardSquares[outputSquareNumber];
                if (s1.hasPiece)
                {
                    if (s1 == s2)
                    {
                        Console.Clear();
                        Console.WriteLine("Cannot skip a turn");
                        DrawBoard(inputBoard);
                    }
                    else if (inputBoard.isWhiteTurn == s1.occupyingPiece.isWhitePiece)
                    {
                        bool legalMove = CheckLegalMove(inputBoard, inputSquareNumber, outputSquareNumber);

                        if (legalMove)
                        {
                            //this code is to confirm that the last move did NOT put the player's king in check.
                            //********THIS IS CURRENTLY NOT WORKING AS EXPECTED
                            //need to make a clone of the input board rather than a new reference to the old one...
                            //instead going creating a move object, the board will have a list.  
                            //this will allow us to use the last move to undo moves that put/leave own king in check.
                            Move newMove = new Move();
                            newMove.StartingSquare = inputSquareNumber;
                            newMove.TargetSquare = outputSquareNumber;
                            newMove.MovingPiece = s1.occupyingPiece;
                            newMove.WasTargetSquareEmpty = s2.hasPiece;
                            if (s2.hasPiece)
                            {
                                newMove.CapturedPiece = s2.occupyingPiece;
                            }
                            newMove.StartingRank = s1.rank;
                            newMove.StartingFile = s1.file;
                            newMove.TargetRank = s2.rank;
                            newMove.TargetFile = s2.file;
                            newMove.HasThisPieceMovedBefore = inputBoard.BoardSquares[inputSquareNumber].occupyingPiece.hasMoved;


                            inputBoard.BoardSquares[outputSquareNumber].occupyingPiece = s1.occupyingPiece;
                            inputBoard.BoardSquares[outputSquareNumber].hasPiece = true;
                            inputBoard.BoardSquares[outputSquareNumber].occupyingPiece.hasMoved = true;
                            inputBoard.BoardSquares[inputSquareNumber].hasPiece = false;
                            //special rules for castling move here?

                            bool didIJustPutMyOwnKingInCheck = IsKingInCheck(inputBoard, inputBoard.isWhiteTurn);
                            bool didIJustCheckTheOpposingKing = IsKingInCheck(inputBoard, !inputBoard.isWhiteTurn);

                            if (didIJustPutMyOwnKingInCheck == false)
                            {
                                //any code related to pieces that were just removed goes here


                                if (inputBoard.isWhiteTurn)
                                {
                                    inputBoard.whiteInCheck = false;
                                    if(didIJustCheckTheOpposingKing)
                                    {
                                        inputBoard.blackInCheck = true;
                                    }
                                }
                                else
                                {
                                    inputBoard.blackInCheck = false;
                                    if (didIJustCheckTheOpposingKing)
                                    {
                                        inputBoard.whiteInCheck = true;
                                    }
                                }

                                inputBoard.isWhiteTurn = !inputBoard.isWhiteTurn;
                                inputBoard.MoveList.Add(newMove);

                                Console.Clear();
                                Console.WriteLine($"moved {s1.occupyingPiece.pieceSymbol} from {s1.file}{s1.rank} to { s2.file}{ s2.rank}");
                                DrawBoard(inputBoard);
                                turnInProgress = false;
                            }
                            else
                            {
                                //use the move object to undo the change
                                //inputBoard.BoardSquares[outputSquareNumber].occupyingPiece = s1.occupyingPiece;
                                //inputBoard.BoardSquares[outputSquareNumber].hasPiece = true;
                                //inputBoard.BoardSquares[outputSquareNumber].occupyingPiece.hasMoved = true;
                                //inputBoard.BoardSquares[inputSquareNumber].hasPiece = false;
                                inputBoard.BoardSquares[newMove.StartingSquare].hasPiece = true;
                                newMove.MovingPiece.hasMoved = newMove.HasThisPieceMovedBefore;
                                inputBoard.BoardSquares[newMove.StartingSquare].occupyingPiece = newMove.MovingPiece;
                                inputBoard.BoardSquares[newMove.TargetSquare].hasPiece = newMove.WasTargetSquareEmpty;
                                if (!newMove.WasTargetSquareEmpty)
                                {
                                    inputBoard.BoardSquares[newMove.TargetSquare].occupyingPiece = newMove.CapturedPiece;
                                }



                                Console.Clear();
                                Console.WriteLine($"{s1.occupyingPiece.pieceSymbol} cannot move from {s1.file}{s1.rank} to { s2.file}{ s2.rank} " +
                                    $"because the king will be in check");
                                DrawBoard(inputBoard);
                            }

                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine($"{s1.occupyingPiece.pieceSymbol} cannot move from {s1.file}{s1.rank} to { s2.file}{ s2.rank} ");
                            DrawBoard(inputBoard);
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("That is not your piece to move");
                        DrawBoard(inputBoard);
                    }

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("No piece to move from that square");
                    DrawBoard(inputBoard);
                }
            }
        }



        private static void DrawBoard(Board inputBoard)
        {
            int currentRow = 0;
            string whoseTurn = "";
            if (inputBoard.isWhiteTurn)
            {
                whoseTurn = "White to move";
                if (inputBoard.whiteInCheck)
                {
                    whoseTurn = "Check. " + whoseTurn;
                }
            }
            else
            {
                whoseTurn = "Black to move";
                if (inputBoard.blackInCheck)
                {
                    whoseTurn = "Check. " + whoseTurn;
                }
            }

            Console.WriteLine(whoseTurn);
            Console.WriteLine(" |abcdefgh");
            Console.Write("-+--------===============");

            foreach (Square s in inputBoard.BoardSquares)
            {

                if (currentRow != s.row)
                {
                    Console.Write("\n" + s.rank + "|");
                    currentRow = s.row;
                }

                if (s.hasPiece)
                {
                    Console.Write(s.occupyingPiece.pieceSymbol);
                }
                else
                {
                    if (s.isWhiteSquare)
                        Console.Write('\u2588');
                    else
                        Console.Write('\u0020');
                }
            }
            Console.WriteLine();
        }
        //this assumes we've already confirmed that a piece has been selected
        //and that it matches the color of the player who is on turn.

        //may modify this to return WHY the move isn't legal, currently just a bool
        private static bool CheckLegalMove(Board inputBoard, int startingSquare, int targetSquare)
        {
            //starting wtih classic army, may expand to chess 2 later.
            if (inputBoard.BoardSquares[startingSquare].occupyingPiece.army == "classic")
            {
                //no classic piece can capture another piece of the same army and color
                if (inputBoard.BoardSquares[targetSquare].hasPiece == true &&
                    (inputBoard.BoardSquares[startingSquare].occupyingPiece.isWhitePiece ==
                    inputBoard.BoardSquares[targetSquare].occupyingPiece.isWhitePiece))
                {
                    //removed for testing purposes temporarily
                    //return false;
                }

                //classic rook
                //can move orthogonally
                //cannot jump pieces
                //cannot capture allied pieces
                //can capture enemy pieces
                if (inputBoard.BoardSquares[startingSquare].occupyingPiece.pieceName == "rook")
                {
                    //organizing this using the row and column of the board square.  11 = top left corner, 88 = bottom right

                    //checking if target squares share a row
                    if (inputBoard.BoardSquares[startingSquare].row == inputBoard.BoardSquares[targetSquare].row)
                    {
                        int squaresToMove = inputBoard.BoardSquares[startingSquare].column -
                            inputBoard.BoardSquares[targetSquare].column;
                        if (squaresToMove > 0)
                        {
                            //makes sure there are no pieces between target square and starting square
                            while (squaresToMove > 1)
                            {
                                squaresToMove--;
                                int squareToCheck = startingSquare - squaresToMove;

                                //Console.WriteLine("checking square" + squareToCheck);
                                //Console.ReadLine();
                                //need to check this line in the if statement
                                if (inputBoard.BoardSquares[squareToCheck].hasPiece)
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            //makes sure there are no pieces between target square and starting square
                            while (squaresToMove < -1)
                            {
                                squaresToMove++;
                                int squareToCheck = startingSquare - squaresToMove;

                                //Console.WriteLine("checking square" + squareToCheck);
                                //Console.ReadLine();
                                //need to check this line in the if statement
                                if (inputBoard.BoardSquares[squareToCheck].hasPiece)
                                {
                                    return false;
                                }
                            }
                        }
                        //assuming no return false has triggered, returns true here
                        return true;

                    }
                    //checking if target squares share a column
                    else if (inputBoard.BoardSquares[startingSquare].column == inputBoard.BoardSquares[targetSquare].column)
                    {
                        int squaresToMove = inputBoard.BoardSquares[startingSquare].row -
                             inputBoard.BoardSquares[targetSquare].row;
                        if (squaresToMove > 0)
                        {
                            //makes sure there are no pieces between target square and starting square
                            while (squaresToMove > 1)
                            {
                                squaresToMove--;
                                int squareToCheck = startingSquare - (8 * squaresToMove);

                                //Console.WriteLine("checking square" + squareToCheck);
                                //Console.ReadLine();
                                //need to check this line in the if statement
                                if (inputBoard.BoardSquares[squareToCheck].hasPiece)
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            //makes sure there are no pieces between target square and starting square
                            while (squaresToMove < -1)
                            {
                                squaresToMove++;
                                int squareToCheck = startingSquare - (8 * squaresToMove);

                                //Console.WriteLine("checking square" + squareToCheck);
                                //Console.ReadLine();
                                //need to check this line in the if statement
                                if (inputBoard.BoardSquares[squareToCheck].hasPiece)
                                {
                                    return false;
                                }
                            }
                        }
                        return true;

                    }
                    //if neither, move is not legal
                    else
                    {
                        return false;
                    }


                    //classic bishop
                    //moves diagonally
                    //cannot jump pieces
                    //cannot capture allied pieces
                    //need to check each space between diagonally
                    //n+9 or n+7 or n-9 or n-7
                    //if current or starting point is on file A or 

                    //classic knight

                    //classic queen


                    //classic king



                }
                //dont forget to check for pawn promotion later
                //dont forget to check for en passant later
                //classic pawn
                //can move forward one space if the space in front of it is not occupied
                //can move forward two spaces if the two spaces in front of it are not occupied AND it has not moved yet
                //can move forward diagonally left or right if and only if the target square is occupied
                //special rule for capturing diagonally IMMEDIATELY after opponent moves two spaces (en passant) NOT IMPLEMENTED
                //special rule for promotion of reaching final rank NOT IMPLEMENTED
                if (inputBoard.BoardSquares[startingSquare].occupyingPiece.pieceName == "pawn")
                {
                    //check for white vs black
                    if (inputBoard.BoardSquares[startingSquare].occupyingPiece.isWhitePiece)
                    {
                        //CANNOT use switch statement here, needs constants.  Gonna need a whole lot of if/else...
                        if (targetSquare == (startingSquare - 16))
                        {
                            if (inputBoard.BoardSquares[startingSquare].occupyingPiece.hasMoved == false
                                && inputBoard.BoardSquares[targetSquare].hasPiece == false
                                && inputBoard.BoardSquares[targetSquare + 8].hasPiece == false)
                            {
                                return true;
                            }
                            return false;
                        }
                        else if (targetSquare == (startingSquare - 8))
                        {
                            if (inputBoard.BoardSquares[targetSquare].hasPiece == false)
                            {
                                return true;
                            }
                            return false;
                        }
                        else if (targetSquare == (startingSquare - 7))
                        {
                            if (inputBoard.BoardSquares[targetSquare].hasPiece
                                && inputBoard.BoardSquares[startingSquare].file != "h")
                            {
                                return true;
                            }
                            return false;
                        }
                        else if (targetSquare == (startingSquare - 9))
                        {
                            if (inputBoard.BoardSquares[targetSquare].hasPiece
                                && inputBoard.BoardSquares[startingSquare].file != "a")
                            {
                                return true;
                            }
                            return false;
                        }
                        else
                        {
                            return false;
                        }


                    }
                    //movement for black pieces
                    else
                    {
                        if (targetSquare == (startingSquare + 16))
                        {
                            if (inputBoard.BoardSquares[startingSquare].occupyingPiece.hasMoved == false
                                && inputBoard.BoardSquares[targetSquare].hasPiece == false
                                && inputBoard.BoardSquares[targetSquare - 8].hasPiece == false)
                            {
                                return true;
                            }
                            return false;
                        }
                        else if (targetSquare == (startingSquare + 8))
                        {
                            if (inputBoard.BoardSquares[targetSquare].hasPiece == false)
                            {
                                return true;
                            }
                            return false;
                        }
                        else if (targetSquare == (startingSquare + 9))
                        {
                            if (inputBoard.BoardSquares[targetSquare].hasPiece
                                && inputBoard.BoardSquares[startingSquare].file != "h")
                            {
                                return true;
                            }
                            return false;
                        }
                        else if (targetSquare == (startingSquare + 7))
                        {
                            if (inputBoard.BoardSquares[targetSquare].hasPiece
                                && inputBoard.BoardSquares[startingSquare].file != "a")
                            {
                                return true;
                            }
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                //classic knight
                //can move in an L shape, 2 spaces horizontally or vertically in any direction, 1 space in the other.
                //barring exceptions for end of the board, can only move to the following squares based on its position:
                //  +15 (2d1l), +17 (2d1r), +6 (1d2l), +10 (1d2r) 
                //  -17 (2u1l), -15 (2u1r), -10 (1u2l), -6 (1u2r)
                // 2l cases need to make sure its not on the A or B file, 2R not on the G or H file
                // 1l cases need to make sure its not on the A file, 1R not on the H file
                if (inputBoard.BoardSquares[startingSquare].occupyingPiece.pieceName == "knight")
                {
                    if (targetSquare == startingSquare - 17)
                    {
                        if (inputBoard.BoardSquares[startingSquare].file != "a"
                            && inputBoard.BoardSquares[startingSquare].rank != "7"
                            && inputBoard.BoardSquares[startingSquare].rank != "8")
                        {
                            return true;
                        }
                        return false;
                    }
                    else if (targetSquare == startingSquare - 15)
                    {
                        if (inputBoard.BoardSquares[startingSquare].file != "h"
                            && inputBoard.BoardSquares[startingSquare].rank != "7"
                            && inputBoard.BoardSquares[startingSquare].rank != "8")
                        {
                            return true;
                        }
                        return false;
                    }
                    else if (targetSquare == startingSquare + 17)
                    {
                        if (inputBoard.BoardSquares[startingSquare].file != "h"
                            && inputBoard.BoardSquares[startingSquare].rank != "1"
                            && inputBoard.BoardSquares[startingSquare].rank != "2")
                        {
                            return true;
                        }
                        return false;
                    }
                    else if (targetSquare == startingSquare + 15)
                    {
                        if (inputBoard.BoardSquares[startingSquare].file != "a"
                            && inputBoard.BoardSquares[startingSquare].rank != "1"
                            && inputBoard.BoardSquares[startingSquare].rank != "2")
                        {
                            return true;
                        }
                        return false;
                    }
                    else if (targetSquare == startingSquare - 10)
                    {
                        if (inputBoard.BoardSquares[startingSquare].file != "a"
                            && inputBoard.BoardSquares[startingSquare].file != "b"
                            && inputBoard.BoardSquares[startingSquare].rank != "8")
                        {
                            return true;
                        }
                        return false;
                    }
                    else if (targetSquare == startingSquare - 6)
                    {
                        if (inputBoard.BoardSquares[startingSquare].file != "g"
                            && inputBoard.BoardSquares[startingSquare].file != "h"
                            && inputBoard.BoardSquares[startingSquare].rank != "8")
                        {
                            return true;
                        }
                        return false;
                    }
                    else if (targetSquare == startingSquare + 10)
                    {
                        if (inputBoard.BoardSquares[startingSquare].file != "g"
                            && inputBoard.BoardSquares[startingSquare].file != "h"
                            && inputBoard.BoardSquares[startingSquare].rank != "1")
                        {
                            return true;
                        }
                        return false;
                    }
                    else if (targetSquare == startingSquare + 6)
                    {
                        if (inputBoard.BoardSquares[startingSquare].file != "a"
                            && inputBoard.BoardSquares[startingSquare].file != "b"
                            && inputBoard.BoardSquares[startingSquare].rank != "1")
                        {
                            return true;
                        }
                        return false;
                    }
                    else
                    {
                        return false;
                    }
                }

                //classic bishop
                //moves diagonally any number of spaces until reaching a piece or the edge of the board
                //cannot jump pieces
                if (inputBoard.BoardSquares[startingSquare].occupyingPiece.pieceName == "bishop")
                {
                    //direction the bishop is moving, with -9,  -7, 7, and 9 representing upleft, upright, downleft, downright
                    int direction = 0;
                    int numberOfSteps = 0;
                    //checks upper right diagonal from starting square
                    for (int i = startingSquare; i >= 0; i -= 9)
                    {
                        if (i == targetSquare)
                        {
                            direction = -9;
                            break;
                        }
                        if (inputBoard.BoardSquares[i].file == "a")
                        {
                            break;
                        }
                        numberOfSteps++;
                    }
                    if (direction == 0)
                    {
                        numberOfSteps = 0;

                        for (int i = startingSquare; i >= 0; i -= 7)
                        {
                            if (i == targetSquare)
                            {
                                direction = -7;
                                break;
                            }
                            if (inputBoard.BoardSquares[i].file == "h")
                            {
                                break;
                            }
                            numberOfSteps++;
                        }
                    }
                    if (direction == 0)
                    {
                        numberOfSteps = 0;

                        for (int i = startingSquare; i <= 63; i += 7)
                        {
                            if (i == targetSquare)
                            {
                                direction = 7;
                                break;
                            }
                            if (inputBoard.BoardSquares[i].file == "a")
                            {
                                break;
                            }
                            numberOfSteps++;
                        }
                    }
                    if (direction == 0)
                    {
                        numberOfSteps = 0;

                        for (int i = startingSquare; i <= 63; i += 9)
                        {
                            if (i == targetSquare)
                            {
                                direction = 9;
                                break;
                            }
                            if (inputBoard.BoardSquares[i].file == "h")
                            {
                                break;
                            }
                            numberOfSteps++;
                        }
                    }
                    if (direction != 0)
                    {
                        //have direction, have number of steps, now need to confirm that all spaces between starting square and target are empty
                        for (int i = 1; i < numberOfSteps; i++)
                        {
                            if (inputBoard.BoardSquares[startingSquare + (i * direction)].hasPiece)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

                //classic queen
                //can move as a bishop OR as a rook
                //defaulting to copying code from both, but may be modified later.
                if (inputBoard.BoardSquares[startingSquare].occupyingPiece.pieceName == "queen")
                {
                    if (inputBoard.BoardSquares[startingSquare].row == inputBoard.BoardSquares[targetSquare].row)
                    {
                        int squaresToMove = inputBoard.BoardSquares[startingSquare].column -
                            inputBoard.BoardSquares[targetSquare].column;
                        if (squaresToMove > 0)
                        {
                            //makes sure there are no pieces between target square and starting square
                            while (squaresToMove > 1)
                            {
                                squaresToMove--;
                                int squareToCheck = startingSquare - squaresToMove;

                                //need to check this line in the if statement
                                if (inputBoard.BoardSquares[squareToCheck].hasPiece)
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            //makes sure there are no pieces between target square and starting square
                            while (squaresToMove < -1)
                            {
                                squaresToMove++;
                                int squareToCheck = startingSquare - squaresToMove;

                                //need to check this line in the if statement
                                if (inputBoard.BoardSquares[squareToCheck].hasPiece)
                                {
                                    return false;
                                }
                            }
                        }
                        //assuming no return false has triggered, returns true here
                        return true;

                    }
                    //checking if target squares share a column
                    else if (inputBoard.BoardSquares[startingSquare].column == inputBoard.BoardSquares[targetSquare].column)
                    {
                        int squaresToMove = inputBoard.BoardSquares[startingSquare].row -
                             inputBoard.BoardSquares[targetSquare].row;
                        if (squaresToMove > 0)
                        {
                            //makes sure there are no pieces between target square and starting square
                            while (squaresToMove > 1)
                            {
                                squaresToMove--;
                                int squareToCheck = startingSquare - (8 * squaresToMove);

                                //need to check this line in the if statement
                                if (inputBoard.BoardSquares[squareToCheck].hasPiece)
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            //makes sure there are no pieces between target square and starting square
                            while (squaresToMove < -1)
                            {
                                squaresToMove++;
                                int squareToCheck = startingSquare - (8 * squaresToMove);


                                //need to check this line in the if statement
                                if (inputBoard.BoardSquares[squareToCheck].hasPiece)
                                {
                                    return false;
                                }
                            }
                        }
                        return true;

                    }
                    else
                    {
                        int direction = 0;
                        int numberOfSteps = 0;
                        //checks upper right diagonal from starting square
                        for (int i = startingSquare; i >= 0; i -= 9)
                        {
                            if (i == targetSquare)
                            {
                                direction = -9;
                                break;
                            }
                            if (inputBoard.BoardSquares[i].file == "a")
                            {
                                break;
                            }
                            numberOfSteps++;
                        }
                        if (direction == 0)
                        {
                            numberOfSteps = 0;

                            for (int i = startingSquare; i >= 0; i -= 7)
                            {
                                if (i == targetSquare)
                                {
                                    direction = -7;
                                    break;
                                }
                                if (inputBoard.BoardSquares[i].file == "h")
                                {
                                    break;
                                }
                                numberOfSteps++;
                            }
                        }
                        if (direction == 0)
                        {
                            numberOfSteps = 0;

                            for (int i = startingSquare; i <= 63; i += 7)
                            {
                                if (i == targetSquare)
                                {
                                    direction = 7;
                                    break;
                                }
                                if (inputBoard.BoardSquares[i].file == "a")
                                {
                                    break;
                                }
                                numberOfSteps++;
                            }
                        }
                        if (direction == 0)
                        {
                            numberOfSteps = 0;

                            for (int i = startingSquare; i <= 63; i += 9)
                            {
                                if (i == targetSquare)
                                {
                                    direction = 9;
                                    break;
                                }
                                if (inputBoard.BoardSquares[i].file == "h")
                                {
                                    break;
                                }
                                numberOfSteps++;
                            }
                        }
                        if (direction != 0)
                        {
                            //have direction, have number of steps, now need to confirm that all spaces between starting square and target are empty
                            for (int i = 1; i < numberOfSteps; i++)
                            {
                                if (inputBoard.BoardSquares[startingSquare + (i * direction)].hasPiece)
                                {
                                    return false;
                                }
                            }
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                //classic king
                //can move one space in any direction (as long as not off the board)
                //UNFINISHED: castling: king moves two spaces, rook moves to its opposite side
                if (inputBoard.BoardSquares[startingSquare].occupyingPiece.pieceName == "king")
                {
                    if (targetSquare >= 0 && targetSquare <= 63)
                    {
                        if (targetSquare == startingSquare + 8 || targetSquare == startingSquare - 8)
                        {
                            return true;
                        }
                        else if (targetSquare == startingSquare - 1
                            || targetSquare == startingSquare - 9
                            || targetSquare == startingSquare + 7)
                        {
                            if (inputBoard.BoardSquares[startingSquare].file == "a")
                            {
                                return false;
                            }
                            return true;
                        }
                        else if (targetSquare == startingSquare + 1
                            || targetSquare == startingSquare - 7
                            || targetSquare == startingSquare + 9)
                        {
                            if (inputBoard.BoardSquares[startingSquare].file == "h")
                            {
                                return false;
                            }
                            return true;
                        }
                        else if (targetSquare == startingSquare - 2)
                        {
                            //attempting to castle queenside
                            //temporarily just having this not work
                            return false;
                        }
                        else if (targetSquare == startingSquare + 2)
                        {
                            //attempting to castle kingside
                            //temporarily just having this not work
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

            }





            //needs a validation for if a king is in check
            //need special case for castling [king moves two spaces toward rook, neither king nor rook can have moved]
            //need special case for pawn capture by en passant [pawn moves diagonally into space that a double-move pawn skipped]
            //need special case for pawn promotion


            return false; ;
        }

        private static bool IsKingInCheck(Board inputBoard, bool isWhiteKing)
        {
            //find square of king of proper color on board
            int kingSquare = -1;
            for(int i = 0; i <= 63; i++)
            {
                if(inputBoard.BoardSquares[i].hasPiece)
                {
                    if (inputBoard.BoardSquares[i].occupyingPiece.isWhitePiece == isWhiteKing
                        && inputBoard.BoardSquares[i].occupyingPiece.pieceName == "king")
                    {
                        kingSquare = i;
                    }
                }

            }
            if(kingSquare == -1)
            {
                return false;
            }
            //check every square on the board.  if it has a piece of the opposite color,
            //then check if that piece can make a legal move to the location of the king.
            for (int i = 0; i < 63; i++)
            {
                if (inputBoard.BoardSquares[i].hasPiece)
                {
                    if (inputBoard.BoardSquares[i].occupyingPiece.isWhitePiece != isWhiteKing)
                    {
                        if (CheckLegalMove(inputBoard, i, kingSquare) == true)
                        {
                            return true;
                        }
                    }
                }
            }



            return false;
            
        }
    }
}

//possible alternative armies
//spider - pawns can move sideways if on the same square as their army color.  queen moves only 2 spaces but kills the attacking piece when captured
//opposition - major and minor pieces (bishop, knight, rook) cannot capture or be captured by the opposing army's equivalent pieces.
//something with a minor drawback but the ability  to freely capture its own pieces