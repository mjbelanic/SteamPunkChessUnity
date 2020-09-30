using System;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Chessboard chessboardPrefab;
    [SerializeField] public string PieceName;
    [SerializeField] public string Color;
    PawnLogic pawnLogic = new PawnLogic();
    RookLogic rookLogic = new RookLogic();
    KnightLogic knightLogic = new KnightLogic();
    BishopLogic bishopLogic = new BishopLogic();
    QueenLogic queenLogic = new QueenLogic();
    KingLogic kingLogic = new KingLogic();
    bool hasMovedAlready;
    int numofMoves = 0;

    internal Dictionary<string, int[]> FindLegalSpaces(int x, int y, Piece[,] p, bool justChecking, int count = 0)
    {

        if (PieceName == "Pawn")        { return pawnLogic.GetPawnMovementSpaces(this, x, y, p, justChecking, count); }
        else if (PieceName == "Rook")   { return rookLogic.GetRookMovementSpaces(this, x, y, p, justChecking, count); }
        else if (PieceName == "Knight") { return knightLogic.GetKnightMovementSpaces(this, x, y, p, count); }
        else if (PieceName == "Bishop") { return bishopLogic.GetBishopMovementSpaces(this, x, y, p, justChecking, count); }
        else if (PieceName == "Queen")  { return queenLogic.GetQueenMovementSpaces(this, x, y, p, justChecking, count); }
        else                            { return kingLogic.GetKingMovementSpaces(this, x, y, p, justChecking); }
    }

    internal bool InBoardDimensions(int x, int y, int x2, int y2, Piece[,] p)
    {
        bool inBoard = true;
        if (y + y2 < 0 || y + y2 >= 8) { inBoard = false; }
        if (x + x2 < 0 || x + x2 >= 8) { inBoard = false; }
        return inBoard;
    }
    internal void ChangeMoveStatus()
    {
        hasMovedAlready = true;
    }
    internal bool GetHasMoveAlready()
    {
        return hasMovedAlready;
    }

    internal void CheckForPromotion(int y)
    {
        if (pawnLogic.CanBePromoted(this, y))
        {
            Debug.Log("Promotion");
            // Make promotiion menu
            // Let user select piece or default to queen?
        }
    }

    internal void IncreaseMoveCounter()
    {
        numofMoves++;
    }

    internal int GetNumOfMoves()
    {
        return numofMoves;
    }

    internal void SetKingStatus(bool status)
    {
        kingLogic.InCheck = status;
    }

    internal bool GetKingStatus()
    {
        return kingLogic.InCheck;
    }

    internal bool MoveDoesNotPutKingInCheck(int startX, int startY, int endX, int endY, Piece[,] boardSpaces)
    {
        if(boardSpaces[endX, endY] == null){
            boardSpaces[endX, endY] = this;
            boardSpaces[startX, startY] = null;
            for (int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if(boardSpaces[i,j] != null && boardSpaces[i,j].PieceName == "King" && boardSpaces[i,j].Color == Color)
                    {
                        Piece king = boardSpaces[i, j];
                        bool safe = king.KingIsSafe(king, boardSpaces, i, j);
                        boardSpaces[startX, startY] = this;
                        boardSpaces[endX, endY] = null;
                        return safe;
                    }
                }
            }
        }
        else
        {
            Piece currentPieceInSpace = boardSpaces[endX, endY];
            boardSpaces[endX, endY] = this;
            boardSpaces[startX, startY] = null;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (boardSpaces[i, j] != null && boardSpaces[i, j].PieceName == "King" && boardSpaces[i, j].Color == Color)
                    {
                        Piece king = boardSpaces[i, j];
                        bool safe = king.KingIsSafe(king, boardSpaces, i, j);
                        boardSpaces[startX, startY] = this;
                        boardSpaces[endX, endY] = currentPieceInSpace;
                        return safe;
                    }
                }
            }

        }
        return false;
    }

    internal bool KingIsSafe(Piece piece, Piece[,] boardSpaces, int x, int y)
    {
        return kingLogic.SpaceIsSafe(piece, boardSpaces, x, y);
    }
}
