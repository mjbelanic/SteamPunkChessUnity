using System.Collections.Generic;
using UnityEngine;

public class PawnLogic : MonoBehaviour
{
    int numberOfMoves = 0;
    private int startX;
    private int startY;
    private int moveCheckCounter;

    public void addMove()
    {
        numberOfMoves++;
    }

    public Dictionary<string, int[]> GetPawnMovementSpaces(Piece piece, int x, int y, Piece[,] boardSpaces, bool justChecking, int moveCheckCounter = 0)
    {
        startX = x;
        startY = y;
        Dictionary<string, int[]> validMoves = new Dictionary<string, int[]>();
        if (justChecking)
        {
            if (piece.Color == "White")
            {
                if (piece.InBoardDimensions(x, y, 1, 1, boardSpaces))  { validMoves.Add("Attack1", new int[] { x + 1, y + 1 }); }
                if (piece.InBoardDimensions(x, y, -1, 1, boardSpaces)) { validMoves.Add("Attack2", new int[] { x - 1, y + 1 }); }
            }
            else
            {
                if (piece.InBoardDimensions(x, y, 1, -1, boardSpaces))  { validMoves.Add("Attack1", new int[] { x + 1, y - 1 }); }
                if (piece.InBoardDimensions(x, y, -1, -1, boardSpaces)) { validMoves.Add("Attack2", new int[] { x - 1, y - 1 }); }
            }
            return validMoves;
        }

        if (piece.Color == "White")
        {
            if (piece.InBoardDimensions(x, y, 0, 1, boardSpaces))
            {
                if (!piece.GetHasMoveAlready() && boardSpaces[x, y + 2] == null && boardSpaces[x, y + 1] == null)
                {
                    if (piece.MoveDoesNotPutKingInCheck(startX, startY, x, y + 2, boardSpaces))
                    {
                        validMoves.Add("Move1", new int[] { x, y + 2 });
                    }
                }
                if (boardSpaces[x + 0, y + 1] == null)
                {
                    if (piece.MoveDoesNotPutKingInCheck(startX, startY, x, y + 1, boardSpaces))
                    {
                        validMoves.Add("Move2", new int[] { x, y + 1 });
                    }
                }
            }
            if (piece.InBoardDimensions(x, y, 1, 1, boardSpaces))
            {
                if (boardSpaces[x + 1, y + 1] != null)
                {
                    Piece otherPiece = boardSpaces[x + 1, y + 1];
                    if (otherPiece.Color != piece.Color && piece.MoveDoesNotPutKingInCheck(startX, startY, x + 1, y + 1, boardSpaces))
                    {
                        validMoves.Add("Attack1", new int[] { x + 1, y + 1 });
                    }
                }
                else if(boardSpaces[x + 1, y + 1] == null && boardSpaces[x + 1, y] != null && y + 1 != 6)
                {
                    Piece otherPiece = boardSpaces[x + 1, y];
                    if (EnPassantValid(piece, otherPiece) && piece.MoveDoesNotPutKingInCheck(startX, startY, x + 1, y + 1, boardSpaces))
                    {
                        validMoves.Add("EnPassant1", new int[] { x + 1, y + 1 });
                    }
                }
            }
            if (piece.InBoardDimensions(x, y, -1, 1, boardSpaces))
            {
                if (boardSpaces[x - 1, y + 1] != null)
                {
                    Piece otherPiece = boardSpaces[x - 1, y + 1];
                    if (otherPiece.Color != piece.Color && piece.MoveDoesNotPutKingInCheck(startX, startY, x - 1, y + 1, boardSpaces))
                    {
                        validMoves.Add("Attack2", new int[] { x - 1, y + 1 });
                    }
                }
                else if (boardSpaces[x - 1, y + 1] == null && boardSpaces[x - 1, y] != null && y + 1 != 6)
                {
                    Piece otherPiece = boardSpaces[x - 1, y];
                    if (EnPassantValid(piece, otherPiece) && piece.MoveDoesNotPutKingInCheck(startX, startY, x - 1, y + 1, boardSpaces))
                    {
                        validMoves.Add("EnPassant2", new int[] { x - 1, y + 1 });
                    }
                }
            }
        }
        else
        {
            if (piece.InBoardDimensions(x, y, 0, -1, boardSpaces))
            {
                if (!piece.GetHasMoveAlready() && boardSpaces[x, y - 2] == null && boardSpaces[x, y - 1] == null)
                {
                    if (piece.MoveDoesNotPutKingInCheck(startX, startY, x, y - 2, boardSpaces))
                    {
                        validMoves.Add("Move1", new int[] { x, y - 2 });
                    }
                }
                if (boardSpaces[x + 0, y - 1] == null)
                {
                    if (piece.MoveDoesNotPutKingInCheck(startX, startY, x, y - 1, boardSpaces))
                    {
                        validMoves.Add("Move2", new int[] { x, y - 1 });
                    }
                }
            }
            if (piece.InBoardDimensions(x, y, 1, -1, boardSpaces))
            {
                if (boardSpaces[x + 1, y - 1] != null)
                {
                    Piece otherPiece = boardSpaces[x + 1, y - 1];
                    if (otherPiece.Color != piece.Color && piece.MoveDoesNotPutKingInCheck(startX, startY, x + 1, y - 1, boardSpaces))
                    {
                        validMoves.Add("Attack1", new int[] { x + 1, y - 1 });
                    }
                }
                else if (boardSpaces[x + 1, y - 1] == null && boardSpaces[x + 1, y] != null && y - 1 != 1)
                {
                    Piece otherPiece = boardSpaces[x + 1, y];
                    if (EnPassantValid(piece, otherPiece) && piece.MoveDoesNotPutKingInCheck(startX, startY, x + 1, y - 1, boardSpaces))
                    {
                        validMoves.Add("EnPassant1", new int[] { x + 1, y - 1 });
                    }
                }
            }
            if (piece.InBoardDimensions(x, y, -1, -1, boardSpaces))
            {
                if (boardSpaces[x - 1, y - 1] != null)
                {
                    Piece otherPiece = boardSpaces[x - 1, y - 1];
                    if (otherPiece.Color != piece.Color && piece.MoveDoesNotPutKingInCheck(startX, startY, x - 1, y - 1, boardSpaces))
                    {
                        validMoves.Add("Attack2", new int[] { x - 1, y - 1 });
                    }
                }
                else if (boardSpaces[x - 1, y - 1] == null && boardSpaces[x - 1, y] != null && y -1 != 1)
                {
                    Piece otherPiece = boardSpaces[x - 1, y];
                    if (EnPassantValid(piece, otherPiece) && piece.MoveDoesNotPutKingInCheck(startX, startY, x - 1, y - 1, boardSpaces))
                    {
                        validMoves.Add("EnPassant2", new int[] { x - 1, y - 1 });
                    }
                }
            }
        }
        return validMoves;
    }

    private static bool EnPassantValid(Piece piece, Piece otherPiece)
    {
        return otherPiece.PieceName == "Pawn"  &&
               otherPiece.GetNumOfMoves() == 1 &&
               otherPiece.Color != piece.Color;
    }

    internal bool CanBePromoted(Piece p, int y)
    {
        if (p.Color == "White" && y == 7) { return true; }
        if (p.Color == "Black" && y == 0) { return true; }
        return false;
    }
}
