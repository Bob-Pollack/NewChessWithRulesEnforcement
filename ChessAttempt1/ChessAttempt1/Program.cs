using System;

namespace ChessAttempt1
{
    class Program
    {
        static void Main(string[] args)
        {
            //allows chess pieces to show up in console
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("CHESS ATTEMPT 1");
            //creates a board
            Board b1 = new Board();
            //fills the board with the squares of a standard chess game.
            //the ones are for classic chess armies, rules may be expanded in the future
            b1.fillBoard(1, 1);

            //draws the board based on the current state of play
            DrawBoard(b1);
            //loops through the taketurn function until the game over flag is triggered
            while (b1.isGameOver == false)
            {
                TakeTurn(b1);
            }

            Console.WriteLine(b1.winMessage);
            Console.ReadLine();

        }

        private static void TakeTurn(Board inputBoard)
        {
            //this boolean tracks the turn and only flips when a legal move has been confirmed 
            bool turnInProgress = true;
            while (turnInProgress)
            {
                //resets this to default at the start of each move attempt.  special case used for pawn promotion, en passant, and castling.
                inputBoard.SpecialCase = "none";
                //tentative input system for the console program variant
                //creates a loop that gets input from the player.  confirms that the player has selected two legal squares on the board
                bool goodInput = false;
                int inputRowNumber = 0;
                int outputRowNumber = 0;
                int inputColumnNumber = 0;
                int outputColumnNumber = 0;
                while (goodInput == false)
                {
                    //takes input for the row of the piece to move.  rows 1-8 base on standard chess rank
                    Console.WriteLine("select piece to move");
                    Console.WriteLine("Row:");
                    string inputRow = Console.ReadLine().ToLower();
                    Int32.TryParse(inputRow, out inputRowNumber);

                    //takes input for the column of the peice to move.  must be a letter a through h for the standard chess files
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

                    //same system as the previous two, this time for the target square
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

                    //special case, if you type "log" at the start, it will go to a seperate system and show game log, then return here after input
                    if (inputRow == "log")
                    {
                        Console.Clear();
                        ShowLog(inputBoard);
                        Console.Clear();
                        Console.WriteLine("Resuming game");
                        DrawBoard(inputBoard);
                    }
                    else if (inputRow == "undo")
                    {
                        Console.Clear();
                        if (inputBoard.MoveList.Count > 0)
                        {
                            UndoMove(inputBoard, inputBoard.MoveList[inputBoard.MoveList.Count - 1]);
                            Console.WriteLine("undo successful.");
                            DrawBoard(inputBoard);
                        }
                        else
                        {
                            Console.WriteLine("undo unsuccessful, no moves to undo");
                            DrawBoard(inputBoard);
                        }
                    }
                    //confirms that legal user input has been 
                    else if (inputColumnNumber != 0 && outputColumnNumber != 0
                        && inputRowNumber > 0 && outputRowNumber > 0
                        && inputRowNumber < 9 && outputRowNumber < 9)
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

                //get the square on the board that the player chose as their square with their piece to move
                int inputSquareNumber = 0;
                inputSquareNumber = 8 * (8 - inputRowNumber) + inputColumnNumber - 1;
                //get the square on the board that the player chose to move their piece to
                int outputSquareNumber = 0;
                outputSquareNumber = 8 * (8 - outputRowNumber) + outputColumnNumber - 1;

                //gets the squares from the board that have those square numbers
                Square s1 = inputBoard.BoardSquares[inputSquareNumber];
                Square s2 = inputBoard.BoardSquares[outputSquareNumber];
                //confirms that there is a piece on the square the player selected.  if not, loop back to start for more player input
                if (s1.hasPiece)
                {
                    //if the player chose the same square twice, sends them back to start for new input
                    if (s1 == s2)
                    {
                        Console.Clear();
                        Console.WriteLine("Cannot skip a turn");
                        DrawBoard(inputBoard);
                    }
                    //confirms that the piece on the selected square is the same color as the player whose turn it is
                    else if (inputBoard.isWhiteTurn == s1.occupyingPiece.IsWhitePiece)
                    {
                        //calls check legal move function to confirm that the selected piece is allowed to move to the target square
                        //does not check if the move would leave that player's king in check
                        bool legalMove = CheckLegalMove(inputBoard, inputSquareNumber, outputSquareNumber);

                        if (legalMove)
                        {
                            //makes a move object to track everything needed to make, log, or undo the move
                            //***possibly break this out into seperate function
                            Move newMove = new Move();                            
                            newMove.StartingSquare = inputSquareNumber;
                            newMove.TargetSquare = outputSquareNumber;
                            newMove.MovingPiece = s1.occupyingPiece;
                            newMove.WasTargetSquareEmpty = !s2.hasPiece;
                            if (s2.hasPiece)
                            {
                                newMove.CapturedPiece = s2.occupyingPiece;
                            }
                            newMove.StartingRank = s1.rank;
                            newMove.StartingFile = s1.file;
                            newMove.TargetRank = s2.rank;
                            newMove.TargetFile = s2.file;
                            newMove.HasThisPieceMovedBefore = inputBoard.BoardSquares[inputSquareNumber].occupyingPiece.HasMoved;
                            newMove.SpecialCase = inputBoard.SpecialCase;
                            
                            //moves the piece from the starting square to the target square.
                            //***may need to store the recently captured piece somehow for future use with the captured pieces list
                            inputBoard.BoardSquares[outputSquareNumber].occupyingPiece = s1.occupyingPiece;
                            inputBoard.BoardSquares[outputSquareNumber].hasPiece = true;
                            inputBoard.BoardSquares[outputSquareNumber].occupyingPiece.HasMoved = true;
                            inputBoard.BoardSquares[inputSquareNumber].hasPiece = false;
                            if  (inputBoard.SpecialCase != "none")
                            {
                                //***special rules for castling, en passant, and pawn promotion should go here
                                if (inputBoard.SpecialCase == "castling queenside")
                                {
                                    //move the rook from the queenside corner to the right of the king's new position
                                    inputBoard.BoardSquares[inputSquareNumber - 1].occupyingPiece =
                                        inputBoard.BoardSquares[inputSquareNumber - 4].occupyingPiece;
                                    inputBoard.BoardSquares[inputSquareNumber - 1].hasPiece = true;
                                    inputBoard.BoardSquares[inputSquareNumber - 1].occupyingPiece.HasMoved = true;
                                    inputBoard.BoardSquares[inputSquareNumber - 4].hasPiece = false;
                                }
                                else if (inputBoard.SpecialCase == "castling kingside")
                                {
                                    inputBoard.BoardSquares[inputSquareNumber + 1].occupyingPiece =
                                       inputBoard.BoardSquares[inputSquareNumber + 3].occupyingPiece;
                                    inputBoard.BoardSquares[inputSquareNumber + 1].hasPiece = true;
                                    inputBoard.BoardSquares[inputSquareNumber + 1].occupyingPiece.HasMoved = true;
                                    inputBoard.BoardSquares[inputSquareNumber + 3].hasPiece = false;
                                }
                                else if (inputBoard.SpecialCase == "pawn promotion")
                                {
                                    //check if this move leaves allied king in check,
                                    //otherwise we would have to undo after having player input for the promotion
                                    if(IsKingInCheck(inputBoard, inputBoard.isWhiteTurn) == false)
                                    {
                                        //loop until we have proper user input
                                        bool goodPromotionInput = false;
                                        while(goodPromotionInput == false)
                                        {
                                            //clear console, draw the board, request user input
                                            Console.Clear();
                                            DrawBoard(inputBoard);
                                            Console.WriteLine("pawn has reached the edge of the board.  please promote to" +
                                                "knight (k), bishop (b), rook (r), or queen (q)");
                                            string promotionInput = Console.ReadLine().ToLower();
                                            //make sure ther is some input to avoid out of bounds error
                                            if (promotionInput != "")
                                            {
                                                goodPromotionInput = true;
                                                //create a new piece to put on the square based on the inputs
                                                Piece PromotedPiece = new Piece();
                                                string currentArmy = GetArmy(inputBoard, inputBoard.isWhiteTurn);

                                                //check first character of user input for proper letter
                                                if (promotionInput[0] == 'k')
                                                {
                                                    //fills in details for the new piece, then places it on the board replacing the pawn
                                                    PromotedPiece.AddPiece("knight", currentArmy, inputBoard.isWhiteTurn);
                                                    PromotedPiece.HasMoved = true;
                                                    inputBoard.BoardSquares[outputSquareNumber].occupyingPiece = PromotedPiece;
                                                }
                                                else if (promotionInput[0] == 'b')
                                                {
                                                    PromotedPiece.AddPiece("bishop", currentArmy, inputBoard.isWhiteTurn);
                                                    PromotedPiece.HasMoved = true;
                                                    inputBoard.BoardSquares[outputSquareNumber].occupyingPiece = PromotedPiece;
                                                }
                                                else if (promotionInput[0] == 'r')
                                                {
                                                    PromotedPiece.AddPiece("rook", currentArmy, inputBoard.isWhiteTurn);
                                                    PromotedPiece.HasMoved = true;
                                                    inputBoard.BoardSquares[outputSquareNumber].occupyingPiece = PromotedPiece;
                                                }
                                                else if (promotionInput[0] == 'q')
                                                {
                                                    PromotedPiece.AddPiece("queen", currentArmy, inputBoard.isWhiteTurn);
                                                    PromotedPiece.HasMoved = true;
                                                    inputBoard.BoardSquares[outputSquareNumber].occupyingPiece = PromotedPiece;
                                                }
                                                else
                                                {
                                                    goodPromotionInput = false;
                                                }

                                            }
                                        }
                                    }
                                }
                            }

                            //determine if the player put or left their king in check (illegal move) or put the opposing king in check
                            bool didIJustPutMyOwnKingInCheck = IsKingInCheck(inputBoard, inputBoard.isWhiteTurn);
                            bool didIJustCheckTheOpposingKing = IsKingInCheck(inputBoard, !inputBoard.isWhiteTurn);

                            //last bit of information for the new move created above
                            newMove.Check = didIJustCheckTheOpposingKing;

                            //confirm that the move was truly legal
                            if (didIJustPutMyOwnKingInCheck == false)
                            {
                                //***any code related to pieces that were just removed goes here

                                //updates the board for the state of check of both sides
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

                                //changes whose turn it is
                                inputBoard.isWhiteTurn = !inputBoard.isWhiteTurn;
                                //adds the new move to the list of moves on the board
                                inputBoard.MoveList.Add(newMove);
                                //redraws the board
                                string outputMessage = $"moved {s1.occupyingPiece.PieceSymbol} from {s1.file}{s1.rank} to { s2.file}{ s2.rank}";
                                if (inputBoard.SpecialCase == "castling queenside")
                                {
                                    outputMessage = outputMessage + " castling queenside";
                                }
                                else if (inputBoard.SpecialCase == "castling kingside")
                                {
                                    outputMessage = outputMessage + " castling kingside";
                                }
                                else if (inputBoard.SpecialCase == "pawn promotion")
                                {
                                    char newPieceSymbol = s2.occupyingPiece.PieceSymbol;
                                    outputMessage = outputMessage + $" and promotes to {newPieceSymbol}";
                                }
                                Console.Clear();
                                Console.WriteLine(outputMessage);
                                DrawBoard(inputBoard);
                                //ends the loop for this turn
                                turnInProgress = false;
                            }
                            //undo the move using the move object if the move would place or leave the king of the moving player in check
                            else
                            {
                                //use the move object to undo the change
                                UndoMove(inputBoard, newMove);

                                //message about illegal move because king is in check
                                Console.Clear();
                                Console.WriteLine($"{s1.occupyingPiece.PieceSymbol} cannot move from {s1.file}{s1.rank} to { s2.file}{ s2.rank} " +
                                    $"because the king will be in check");
                                DrawBoard(inputBoard);
                            }

                        }
                        else
                        {
                            //message about a piece being unable to legally move to the target square
                            Console.Clear();
                            Console.WriteLine($"{s1.occupyingPiece.PieceSymbol} cannot move from {s1.file}{s1.rank} to { s2.file}{ s2.rank} ");
                            DrawBoard(inputBoard);
                        }
                    }
                    else
                    {
                        //message about attempting to move the opponent's piece
                        Console.Clear();
                        Console.WriteLine("That is not your piece to move");
                        DrawBoard(inputBoard);
                    }

                }
                else
                {
                    //message about a starting square not having an active piece
                    Console.Clear();
                    Console.WriteLine("No piece to move from that square");
                    DrawBoard(inputBoard);
                }
            }
        }

        private static string GetArmy(Board inputBoard, bool isWhiteArmy)
        {
            //find any piece of the proper army color, return its army parameter
            for(int i = 0; i <= 63; i++)
            {
                if(inputBoard.BoardSquares[i].hasPiece)
                {
                    if(inputBoard.BoardSquares[i].occupyingPiece.IsWhitePiece == isWhiteArmy)
                    {
                        return inputBoard.BoardSquares[i].occupyingPiece.Army;
                    }
                }
            }
            return "error: no piece of that color exists on the board.  this text should never appear.";
        }

        private static void UndoMove(Board inputBoard, Move moveToUndo)
        {
            //uses the move we've received to undo the board to the previous state
            //***if we're going to use the captured pieces lists, will have to do something here to update them
            inputBoard.BoardSquares[moveToUndo.StartingSquare].hasPiece = true;
            moveToUndo.MovingPiece.HasMoved = moveToUndo.HasThisPieceMovedBefore;
            inputBoard.BoardSquares[moveToUndo.StartingSquare].occupyingPiece = moveToUndo.MovingPiece;
            inputBoard.BoardSquares[moveToUndo.TargetSquare].hasPiece = !moveToUndo.WasTargetSquareEmpty;
            if (!moveToUndo.WasTargetSquareEmpty)
            {
                inputBoard.BoardSquares[moveToUndo.TargetSquare].occupyingPiece = moveToUndo.CapturedPiece;
            }
            //additional fixes to undo special case moves (castling, pawn promotion, en passant)
            if (moveToUndo.SpecialCase != "none")
            {
                //if castling, need to reset the rook to its default position and hasMoved flag
                if (moveToUndo.SpecialCase == "castling queenside")
                {
                    inputBoard.BoardSquares[moveToUndo.StartingSquare - 1].occupyingPiece.HasMoved = false;
                    inputBoard.BoardSquares[moveToUndo.StartingSquare - 4].occupyingPiece =
                        inputBoard.BoardSquares[moveToUndo.StartingSquare - 1].occupyingPiece;
                    inputBoard.BoardSquares[moveToUndo.StartingSquare - 1].hasPiece = false;
                    inputBoard.BoardSquares[moveToUndo.StartingSquare - 4].hasPiece = true;
                }
                else if (moveToUndo.SpecialCase == "castling kingside")
                {
                    inputBoard.BoardSquares[moveToUndo.StartingSquare + 1].occupyingPiece.HasMoved = false;
                    inputBoard.BoardSquares[moveToUndo.StartingSquare + 3].occupyingPiece =
                        inputBoard.BoardSquares[moveToUndo.StartingSquare + 1].occupyingPiece;
                    inputBoard.BoardSquares[moveToUndo.StartingSquare + 1].hasPiece = false;
                    inputBoard.BoardSquares[moveToUndo.StartingSquare + 3].hasPiece = true;
                }
                //**other special case (en passant, pawn promotion)
            }
            
            //making sure we dont get an out of bounds error
            if(inputBoard.MoveList.Count > 0)
            {
                //checks if we're undoing a move from the move list.  If so, remove it from the move list
                //and change the board situation (whose turn it is, is player in check, etc).
                if (moveToUndo == inputBoard.MoveList[inputBoard.MoveList.Count - 1])
                {
                    //scoreList.RemoveAt(scoreList.Count-1);
                    inputBoard.MoveList.RemoveAt(inputBoard.MoveList.Count - 1);
                    inputBoard.isWhiteTurn = !inputBoard.isWhiteTurn;
                    inputBoard.whiteInCheck = IsKingInCheck(inputBoard, true);
                    inputBoard.blackInCheck = IsKingInCheck(inputBoard, false);

                }
            }
        }

        //method to display a log of moves taken in the game
        private static void ShowLog(Board inputBoard)
        {            
            Console.WriteLine("Game Log");
            //default if no moves taken
            if (inputBoard.MoveList.Count == 0)
            {
                Console.WriteLine("No moves have yet been played");
            }
            //loops through the move list
            for ( int i = 0; i < inputBoard.MoveList.Count; i++ )
            {
                //output modification for if the move created a "check" gamestate
                string causingCheck = "";
                if (inputBoard.MoveList[i].Check)
                {
                    causingCheck = ", check";
                }
                //output modification for if the move captured an enemy piece
                string capturingPiece = "";
                if ( inputBoard.MoveList[i].WasTargetSquareEmpty == false )
                {
                    capturingPiece = $" capturing {inputBoard.MoveList[i].CapturedPiece.PieceSymbol}";
                }
                //output display
                Console.WriteLine($"{inputBoard.MoveList[i].MovingPiece.PieceSymbol} {inputBoard.MoveList[i].StartingFile}{inputBoard.MoveList[i].StartingRank}" +
                    $" to {inputBoard.MoveList[i].TargetFile}{inputBoard.MoveList[i].TargetRank}" +
                    $"{capturingPiece}{causingCheck}.");
            }
            //get user confirmation before displaying the board again
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();

        }

        //method to draw the board
        private static void DrawBoard(Board inputBoard)
        {
            int currentRow = 0;
            //display whose turn it is and if their king is in check
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
            //default lines.  extra = signs are there to prevent odd unicode formatting issues
            Console.WriteLine(whoseTurn);
            Console.WriteLine(" |abcdefgh");
            Console.Write("-+--------===============");

            //display each square in the board
            foreach (Square s in inputBoard.BoardSquares)
            {
                //extra drawn at the start of each row of the board
                if (currentRow != s.row)
                {
                    Console.Write("\n" + s.rank + "|");
                    currentRow = s.row;
                }
                //displays the piece symbol if there is a piece on the suqare
                if (s.hasPiece)
                {
                    Console.Write(s.occupyingPiece.PieceSymbol);
                }
                //displays a white or black square if no piece there to occupy it - functional but ugly in console
                else
                {
                    if (s.isWhiteSquare)
                        Console.Write('\u2588');
                    else
                        Console.Write('\u0020');
                }
            }
            //ends the line once we finish the final square
            Console.WriteLine();
        }

        //returns whether the selected piece can move to the target square
        //this does NOT check whether this move would leave the player's king in check.
        //this assumes that we've already confirmed that a piece has been selected that matches the color of the player whose turn it is
        private static bool CheckLegalMove(Board inputBoard, int startingSquare, int targetSquare)
        {
            //starting wtih classic army, may expand to alternate variants later.  if so, will use a superclass or interface
            if (inputBoard.BoardSquares[startingSquare].occupyingPiece.Army == "classic")
            {
                //no classic piece can capture another piece of the same army and color
                if (inputBoard.BoardSquares[targetSquare].hasPiece == true &&
                    (inputBoard.BoardSquares[startingSquare].occupyingPiece.IsWhitePiece ==
                    inputBoard.BoardSquares[targetSquare].occupyingPiece.IsWhitePiece))
                {
                    //removed for testing purposes temporarily
                    //return false;
                }

                //standard rook
                if (inputBoard.BoardSquares[startingSquare].occupyingPiece.PieceName == "rook")
                {

                    //checking if target squares share a row.  rows are 1-8 down the board, in the opposite pattern as the ranks 
                    if (inputBoard.BoardSquares[startingSquare].row == inputBoard.BoardSquares[targetSquare].row)
                    {
                        //comparing the column values, which are the files a-h in number form (1-8)
                        int squaresToMove = inputBoard.BoardSquares[startingSquare].column -
                            inputBoard.BoardSquares[targetSquare].column;
                        //checks if the target is to the left of the starting square
                        if (squaresToMove > 0)
                        {
                            //makes sure there are no pieces between target square and starting square
                            while (squaresToMove > 1)
                            {
                                //checks each square between the starting point and the target to confirm that they do not have pieces
                                squaresToMove--;
                                int squareToCheck = startingSquare - squaresToMove;
                                if (inputBoard.BoardSquares[squareToCheck].hasPiece)
                                {
                                    return false;
                                }
                            }
                        }
                        //checks if the target is to the right of the starting square
                        else
                        {
                            //makes sure there are no pieces between target square and starting square
                            while (squaresToMove < -1)
                            {
                                //checks each square between the starting point and the target to confirm that they do not have pieces
                                squaresToMove++;
                                int squareToCheck = startingSquare - squaresToMove;
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
                }
                //standard pawn
                //special rule for capturing diagonally IMMEDIATELY after opponent pawn moves two spaces (en passant) NOT IMPLEMENTED YET
                //special rule for promotion of reaching final rank NOT IMPLEMENTED YET
                if (inputBoard.BoardSquares[startingSquare].occupyingPiece.PieceName == "pawn")
                {
                    //check if the target square is on the back row.  if so, if the move is legal, the pawn will be given the opportunity to promote.
                    if(inputBoard.BoardSquares[targetSquare].rank == "8" || inputBoard.BoardSquares[targetSquare].rank == "1")
                    {
                        inputBoard.SpecialCase = "pawn promotion";
                    }

                    //check for white vs black
                    if (inputBoard.BoardSquares[startingSquare].occupyingPiece.IsWhitePiece)
                    {
                        //checks if trying to move two spaces forward
                        if (targetSquare == (startingSquare - 16))
                        {
                            //confirms that the move is legal
                            if (inputBoard.BoardSquares[startingSquare].occupyingPiece.HasMoved == false
                                && inputBoard.BoardSquares[targetSquare].hasPiece == false
                                && inputBoard.BoardSquares[targetSquare + 8].hasPiece == false)
                            {
                                return true;
                            }
                            return false;
                        }
                        //checks if trying to move one space forward
                        else if (targetSquare == (startingSquare - 8))
                        {
                            if (inputBoard.BoardSquares[targetSquare].hasPiece == false)
                            {                               
                                return true;
                            }
                            return false;
                        }
                        //checks if trying to capture diagonally right
                        else if (targetSquare == (startingSquare - 7))
                        {
                            if (inputBoard.BoardSquares[targetSquare].hasPiece
                                && inputBoard.BoardSquares[startingSquare].file != "h")
                            {
                                return true;
                            }
                            return false;
                        }
                        //checks if trying to capture diagonally left
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
                    //movement for black pieces is the same as the white pieces, just reversed
                    else
                    {
                        if (targetSquare == (startingSquare + 16))
                        {
                            if (inputBoard.BoardSquares[startingSquare].occupyingPiece.HasMoved == false
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
                        //+9 is diagonally right capture for black, +7 is diagonally left
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

                //standard chess knight
                //couldn't come up with a generalizable way to make sure the piece wasn't leaving the board, so I manually checked each direction 
                //based on rank and file.
                //one square up is -8 on the board, one down is +8, one left is -1, one right is +1
                //so  each move is ±16 ±1 or ±8 ±2
                if (inputBoard.BoardSquares[startingSquare].occupyingPiece.PieceName == "knight")
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

                //standard bishop
                if (inputBoard.BoardSquares[startingSquare].occupyingPiece.PieceName == "bishop")
                {
                    //direction the bishop is moving, with -9,  -7, 7, and 9 representing upleft, upright, downleft, downright
                    int direction = 0;
                    int numberOfSteps = 0;
                    //checks upper left diagonal from the starting square
                    for (int i = startingSquare; i >= 0; i -= 9)
                    {
                        //if it finds the target square, sets direction to -9
                        if (i == targetSquare)
                        {
                            direction = -9;
                            break;
                        }
                        //if it reaches the a file, it has reached teh edge of the board.
                        //the for loop automatically breaks if it would be going off the boarrd vertically
                        if (inputBoard.BoardSquares[i].file == "a")
                        {
                            break;
                        }
                        numberOfSteps++;
                    }
                    //only checks the other diagonals if the target has not been found on a previous one
                    if (direction == 0)
                    {
                        //resets the number of steps since the target was not found on the previous diagonal
                        numberOfSteps = 0;
                        //same loop as before, this time checking the upper right diagonal
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

                        //checking the lower left diagonal.  making sure i <= 63 prevents going off the board vertically
                        for (int i = startingSquare; i <= 63; i += 7)
                        {
                            if (i == targetSquare)
                            {
                                direction = 7;
                                break;
                            }
                            //checking the a file to make sure we don't go off the side of the board horizontally
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
                        //checking the lower right diagonal
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
                    //confirms that a direction has been found.  if not, no legal bishop move can be made
                    if (direction != 0)
                    {
                        //have direction, have number of steps, now confirming that all spaces between starting square and target are empty
                        for (int i = 1; i < numberOfSteps; i++)
                        {
                            //directions from before were specifically set to +7, +9, -7, or -9 to work with this loop
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

                //standard queen
                //moves as either a rook or a bishop.  I currently have the code copied from the other two here
                //but plan to refactor it to avoid duplicated code
                if (inputBoard.BoardSquares[startingSquare].occupyingPiece.PieceName == "queen")
                {
                    //rook movement
                    if (inputBoard.BoardSquares[startingSquare].row == inputBoard.BoardSquares[targetSquare].row)
                    {
                        int squaresToMove = inputBoard.BoardSquares[startingSquare].column -
                            inputBoard.BoardSquares[targetSquare].column;
                        if (squaresToMove > 0)
                        {
                            while (squaresToMove > 1)
                            {
                                squaresToMove--;
                                int squareToCheck = startingSquare - squaresToMove;

                                if (inputBoard.BoardSquares[squareToCheck].hasPiece)
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            while (squaresToMove < -1)
                            {
                                squaresToMove++;
                                int squareToCheck = startingSquare - squaresToMove;

                                if (inputBoard.BoardSquares[squareToCheck].hasPiece)
                                {
                                    return false;
                                }
                            }
                        }
                        return true;

                    }
                    else if (inputBoard.BoardSquares[startingSquare].column == inputBoard.BoardSquares[targetSquare].column)
                    {
                        int squaresToMove = inputBoard.BoardSquares[startingSquare].row -
                             inputBoard.BoardSquares[targetSquare].row;
                        if (squaresToMove > 0)
                        {
                            while (squaresToMove > 1)
                            {
                                squaresToMove--;
                                int squareToCheck = startingSquare - (8 * squaresToMove);

                                if (inputBoard.BoardSquares[squareToCheck].hasPiece)
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            while (squaresToMove < -1)
                            {
                                squaresToMove++;
                                int squareToCheck = startingSquare - (8 * squaresToMove);

                                if (inputBoard.BoardSquares[squareToCheck].hasPiece)
                                {
                                    return false;
                                }
                            }
                        }
                        return true;

                    }
                    //bishop movement
                    else
                    {
                        int direction = 0;
                        int numberOfSteps = 0;
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
                //if the king is NOT in check, and it has not moved, and there are open spaces horizontally to a rook and that rook has not moved
                //the king may castle, moving two spaces toward the rook and having the rook move to its opposite side.
                if (inputBoard.BoardSquares[startingSquare].occupyingPiece.PieceName == "king")
                {
                    //confirms targetsquare is on the board
                    if (targetSquare >= 0 && targetSquare <= 63)
                    {
                        //checks squares directly above and below the king
                        if (targetSquare == startingSquare + 8 || targetSquare == startingSquare - 8)
                        {
                            return true;
                        }
                        //checks squares to the left of the king.  cannot be done from the a file
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
                        //checks squares to the right of the king.  cannot be done from the h file
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
                        //attempting to castle queenside
                        else if (targetSquare == startingSquare - 2)
                        {
                            //making sure king is not currently in check
                            if ((inputBoard.isWhiteTurn == true && inputBoard.whiteInCheck == false)
                                || (inputBoard.isWhiteTurn == false && inputBoard.blackInCheck == false))
                            {
                                //checking that king has not moved
                                if (inputBoard.BoardSquares[startingSquare].occupyingPiece.HasMoved == false)
                                {
                                    //checking that the rook has not moved - in seperate if statement to avoid possible out of bounds error
                                    if (inputBoard.BoardSquares[startingSquare - 4].hasPiece == true
                                        && inputBoard.BoardSquares[startingSquare - 4].occupyingPiece.HasMoved == false)
                                    {
                                        //checking that the spaces between the king and rook are empty
                                        if (inputBoard.BoardSquares[startingSquare - 3].hasPiece == false
                                            && inputBoard.BoardSquares[startingSquare - 2].hasPiece == false
                                            && inputBoard.BoardSquares[startingSquare - 1].hasPiece == false)
                                        {
                                            //set special case flag to alert the program that this is not a standard move
                                            inputBoard.SpecialCase = "castling queenside";
                                            return true;
                                        }
                                    }

                                }
                            }
                            return false;
                        }
                        else if (targetSquare == startingSquare + 2)
                        {
                            //making sure king is not currently in check
                            if ((inputBoard.isWhiteTurn == true && inputBoard.whiteInCheck == false)
                                || (inputBoard.isWhiteTurn == false && inputBoard.blackInCheck == false))
                            {
                                //checking that king has not moved
                                if (inputBoard.BoardSquares[startingSquare].occupyingPiece.HasMoved == false)
                                {
                                    //checking that the rook has not moved - in seperate if statement to avoid possible out of bounds error
                                    if (inputBoard.BoardSquares[startingSquare + 3].hasPiece == true
                                        && inputBoard.BoardSquares[startingSquare + 3].occupyingPiece.HasMoved == false)
                                    {
                                        //checking that the spaces between the king and rook are empty
                                        if (inputBoard.BoardSquares[startingSquare + 2].hasPiece == false
                                            && inputBoard.BoardSquares[startingSquare + 1].hasPiece == false)
                                        {
                                            //set special case flag to alert the program that this is not a standard move
                                            inputBoard.SpecialCase = "castling kingside";
                                            return true;
                                        }
                                    }

                                }
                            }
                            return false;
                        }
                        //if not one of the legal king moves
                        else
                        {
                            return false;
                        }
                    }
                }
            }

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
                    if (inputBoard.BoardSquares[i].occupyingPiece.IsWhitePiece == isWhiteKing
                        && inputBoard.BoardSquares[i].occupyingPiece.PieceName == "king")
                    {
                        kingSquare = i;
                    }
                }

            }
            //special case, if there is no king on the board of the proper color (should never happen) program will not crash
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
                    if (inputBoard.BoardSquares[i].occupyingPiece.IsWhitePiece != isWhiteKing)
                    {
                        //uses the CheckLegalMove function with each piece to see if they can legally move to the square the king is on.
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

//rules not yet fully implemented
    //pawn promotion
    //en passant
    //end game 
        //checking for no legal moves (stalemate or checkmate)
        //checking for "same position 3 times"
        //checking for "no pieces or pawn moves for 50 turns"
        //resign
        //offer draw


//possible alternative armies 
//spider - pawns can move sideways if on the same square as their army color.  queen moves only 2 spaces but kills the attacking piece when captured
//opposition - major and minor pieces (bishop, knight, rook) cannot capture or be captured by the opposing army's equivalent pieces.
//something with a minor drawback but the ability  to freely capture its own pieces

    ///******************SOMETHING IS CURRENTLY WRONG WITH THE PROMOTION SYSTEM, ITS STRING IS IN EFFECT TOO OFTEN.  LOOK INTO THIS TOMORROW.