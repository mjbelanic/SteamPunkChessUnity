using System;
using System.Collections.Generic;
using UnityEngine;

public class KingLogic : MonoBehaviour
{
    public bool InCheck { get; set; }
    int startX;
    int startY;
    internal Dictionary<string, int[]> GetKingMovementSpaces(Piece piece, int x, int y, Piece[,] boardSpaces, bool justChecking)
    {
        Dictionary<string, int[]> validMoves = new Dictionary<string, int[]>();
        int newX;
        int newY;
        startX = x;
        startY = y;
        if (piece.InBoardDimensions(x, y, 0, 1, boardSpaces))
        {
            newX = x;
            newY = y + 1;
            FindValidMoves(piece, boardSpaces, ref validMoves, newX, newY, "1", justChecking);
        }
        if (piece.InBoardDimensions(x, y, 1, 0, boardSpaces))
        {
            newX = x + 1;
            newY = y;
            FindValidMoves(piece, boardSpaces, ref validMoves, newX, newY, "2", justChecking);
        }
        if (piece.InBoardDimensions(x, y, 0, -1, boardSpaces))
        {
            newX = x;
            newY = y - 1;
            FindValidMoves(piece, boardSpaces, ref validMoves, newX, newY, "3", justChecking);
        }
        if (piece.InBoardDimensions(x, y, -1, 0, boardSpaces))
        {
            newX = x - 1;
            newY = y;
            FindValidMoves(piece, boardSpaces, ref validMoves, newX, newY, "4", justChecking);
        }
        if (piece.InBoardDimensions(x, y, 1, 1, boardSpaces))
        {
            newX = x + 1;
            newY = y + 1;
            FindValidMoves(piece, boardSpaces, ref validMoves, newX, newY, "5", justChecking);
        }
        if (piece.InBoardDimensions(x, y, 1, -1, boardSpaces))
        {
            newX = x + 1;
            newY = y - 1;
            FindValidMoves(piece, boardSpaces, ref validMoves, newX, newY, "6", justChecking);
        }
        if (piece.InBoardDimensions(x, y, -1, -1, boardSpaces))
        {
            newX = x - 1;
            newY = y - 1;
            FindValidMoves(piece, boardSpaces, ref validMoves, newX, newY, "7", justChecking);
        }
        if (piece.InBoardDimensions(x, y, -1, 1, boardSpaces))
        {
            newX = x - 1;
            newY = y + 1;
            FindValidMoves(piece, boardSpaces, ref validMoves, newX, newY, "8", justChecking);
        }
        if (!justChecking)
        {
            CheckForCastles(piece, boardSpaces, ref validMoves, x, y);
        }
        return validMoves;
    }

    private void CheckForCastles(Piece piece, Piece[,] boardSpaces, ref Dictionary<string, int[]> validMoves, int x, int y)
    {
        int newX1;
        int newX2;
        if (piece.Color == "White" && !piece.GetHasMoveAlready())
        {
            Piece rightCornerPiece = boardSpaces[7, 0];
            Piece leftCornerPiece = boardSpaces[0, 0];
            if (rightCornerPiece != null)
            {
                newX1 = x + 1;
                newX2 = x + 2;
                if (rightCornerPiece.PieceName == "Rook" && !rightCornerPiece.GetHasMoveAlready())
                {
                    if(SpaceIsSafe(piece, boardSpaces, newX1, y) && SpaceIsSafe(piece, boardSpaces, newX2, y))
                    {
                        if (CanCastleBoardRightSide(x, 7, 0, boardSpaces))
                        {
                            validMoves.Add("Castle1", new int[] { newX2, y });
                        }
                    }

                }
            }
            if (leftCornerPiece != null)
            {
                newX1 = x - 1;
                newX2 = x - 2;
                if (leftCornerPiece.PieceName == "Rook" && !leftCornerPiece.GetHasMoveAlready())
                {
                    if(SpaceIsSafe(piece, boardSpaces, newX1, y) && SpaceIsSafe(piece, boardSpaces, newX2, y))
                    {
                        if (CanCastleBoardLeftSide(x, 0, 0, boardSpaces))
                        {
                            validMoves.Add("Castle2", new int[] { newX2, y });
                        }
                    }
                }
            }
        }
        if (piece.Color == "Black" && !piece.GetHasMoveAlready())
        {
            Piece rightCornerPiece = boardSpaces[7, 7];
            Piece leftCornerPiece = boardSpaces[0, 7];
            if (rightCornerPiece != null)
            {
                if (rightCornerPiece.PieceName == "Rook" && !rightCornerPiece.GetHasMoveAlready())
                {
                    newX1 = x + 1;
                    newX2 = x + 2;
                    if (SpaceIsSafe(piece, boardSpaces, newX1, y) && SpaceIsSafe(piece, boardSpaces, newX2, y))
                    {
                        if (CanCastleBoardRightSide(x, 7, 7, boardSpaces))
                        {
                            validMoves.Add("Castle1", new int[] { newX2, y });
                        }
                    }
                }
            }
            if (leftCornerPiece != null)
            {
                if (leftCornerPiece.PieceName == "Rook" && !leftCornerPiece.GetHasMoveAlready())
                {
                    newX1 = x - 1;
                    newX2 = x - 2;
                    if (SpaceIsSafe(piece, boardSpaces, newX1, y) && SpaceIsSafe(piece, boardSpaces, newX2, y))
                    {
                        if (CanCastleBoardLeftSide(x, 0, 7, boardSpaces))
                        {
                            validMoves.Add("Castle2", new int[] { newX2, y });
                        }
                    }                    
                }
            }
        }
    }

    private bool CanCastleBoardLeftSide(int kingX, int rookX, int y, Piece[,] boardSpaces)
    {
        for (int i = kingX - 1; i > rookX; i--)
        {
            if (boardSpaces[i, y] != null)
            {
                return false;
            }
        }
        return true;
    }

    private bool CanCastleBoardRightSide(int kingX,  int rookX, int y, Piece[,] boardSpaces)
    {
        for (int i = kingX + 1; i < rookX; i++)
        {
            if (boardSpaces[i, y] != null)
            {
                return false;
            }
        }
        return true;
    }

    private void FindValidMoves(Piece piece, Piece[,] boardSpaces, ref Dictionary<string, int[]> validMoves, int x, int y, string moveNum, bool justChecking)
    {
        if (!justChecking)
        {
            if (SpaceIsSafe(piece, boardSpaces, x, y))
            {
                if (boardSpaces[x, y] != null)
                {
                    if (boardSpaces[x, y].Color != piece.Color)
                    {
                        validMoves.Add("Attack" + moveNum, new int[] { x, y });
                    }
                }
                else
                {
                    validMoves.Add("Move" + moveNum, new int[] { x, y });
                }
            }
        }
        else
        {
            if (boardSpaces[x, y] != null)
            {
                if (boardSpaces[x, y].Color != piece.Color)
                {
                    validMoves.Add("Attack" + moveNum, new int[] { x, y });
                }
            }
            else
            {
                validMoves.Add("Move" + moveNum, new int[] { x, y });
            }
        }
    }


    internal bool SpaceIsSafe(Piece piece, Piece[,] boardSpaces, int x, int y)
    {
        Dictionary<string, int[]> validEnemyMoves;

        if (boardSpaces[x, y] != null && boardSpaces[x, y].Color != piece.Color)
        {
            Piece currentPieceInSpace = boardSpaces[x, y];
            boardSpaces[x, y] = piece;
            boardSpaces[startX, startY] = null;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece otherPiece = boardSpaces[i, j];
                    if (otherPiece != null && otherPiece.Color != piece.Color)
                    {
                        validEnemyMoves = otherPiece.FindLegalSpaces(i, j, boardSpaces, true, 1);
                        foreach (var value in validEnemyMoves.Values)
                        {
                            if (value[0] == x && value[1] == y)
                            {
                                boardSpaces[startX, startY] = piece;
                                boardSpaces[x, y] = currentPieceInSpace;
                                return false;
                            }
                        }
                    }
                }
            }
            boardSpaces[startX, startY] = piece;
            boardSpaces[x, y] = currentPieceInSpace;
            return true;
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece otherPiece = boardSpaces[i, j];
                    if (otherPiece != null && otherPiece.Color != piece.Color)
                    {
                        validEnemyMoves = otherPiece.FindLegalSpaces(i, j, boardSpaces, true, 1);
                        foreach (var value in validEnemyMoves.Values)
                        {
                            if (value[0] == x && value[1] == y)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }
        return true;
    }
}
