using System;
using System.Collections.Generic;
using UnityEngine;

public class KnightLogic : MonoBehaviour
{
    private int startX;
    private int startY;
    private int moveCheckCounter;
    internal Dictionary<string, int[]> GetKnightMovementSpaces(Piece piece, int x, int y, Piece[,] p, int moveCheckCounter = 0)
    {
        Dictionary<string, int[]> validMoves = new Dictionary<string, int[]>();
        startX = x;
        startY = y;
        int newX;
        int newY;
        if (piece.InBoardDimensions(x, y, 2, 1, p))
        {
            newX = x + 2;
            newY = y + 1;
            if (moveCheckCounter != 0)  { GetMoves(piece, p, ref validMoves, newX, newY, "1"); }
            else                        { FindValidMoves(piece, p, ref validMoves, newX, newY, "1"); }
        }
        if (piece.InBoardDimensions(x, y, 2, -1, p))
        {
            newX = x + 2;
            newY = y - 1;
            if (moveCheckCounter != 0)  { GetMoves(piece, p, ref validMoves, newX, newY, "2"); }
            else                        { FindValidMoves(piece, p, ref validMoves, newX, newY, "2"); }
        }
        if (piece.InBoardDimensions(x, y, -2, 1, p))
        {
            newX = x - 2;
            newY = y + 1;
            if (moveCheckCounter != 0)  { GetMoves(piece, p, ref validMoves, newX, newY, "3"); }
            else                        { FindValidMoves(piece, p, ref validMoves, newX, newY, "3"); }
        }
        if (piece.InBoardDimensions(x, y, -2, -1, p))
        {
            newX = x - 2;
            newY = y - 1;
            if (moveCheckCounter != 0)  { GetMoves(piece, p, ref validMoves, newX, newY, "4"); }
            else                        { FindValidMoves(piece, p, ref validMoves, newX, newY, "4"); }
        }
        if (piece.InBoardDimensions(x, y, 1, 2, p))
        {
            newX = x + 1;
            newY = y + 2;
            if (moveCheckCounter != 0)  { GetMoves(piece, p, ref validMoves, newX, newY, "5"); }
            else                        { FindValidMoves(piece, p, ref validMoves, newX, newY, "5"); }
        }
        if (piece.InBoardDimensions(x, y, 1, -2, p))
        {
            newX = x + 1;
            newY = y - 2;
            if (moveCheckCounter != 0)  { GetMoves(piece, p, ref validMoves, newX, newY, "6"); }
            else                        { FindValidMoves(piece, p, ref validMoves, newX, newY, "6"); }
        }
        if (piece.InBoardDimensions(x, y, -1, 2, p))
        {
            newX = x - 1;
            newY = y + 2;
            if (moveCheckCounter != 0)  { GetMoves(piece, p, ref validMoves, newX, newY, "7"); }
            else                        { FindValidMoves(piece, p, ref validMoves, newX, newY, "7"); }
        }
        if (piece.InBoardDimensions(x, y, -1, -2, p))
        {
            newX = x - 1;
            newY = y - 2;
            if (moveCheckCounter != 0)  { GetMoves(piece, p, ref validMoves, newX, newY, "8"); }
            else                        { FindValidMoves(piece, p, ref validMoves, newX, newY, "8"); }
        }
        return validMoves;
    }

    private void GetMoves(Piece piece, Piece[,] boardSpaces, ref Dictionary<string, int[]> validMoves, int x, int y, string v)
    {
        Piece otherPiece = boardSpaces[x, y];
        if (otherPiece != null)
        {
            if (otherPiece.Color != piece.Color)
            {
                validMoves.Add("Attack" + v, new int[] { x, y });
            }
        }
        else
        {
            validMoves.Add("Move" + v, new int[] { x, y });
        }
    }

    private void FindValidMoves(Piece piece, Piece[,] boardSpaces, ref Dictionary<string, int[]> validMoves, int x, int y, string v)
    {
        Piece otherPiece = boardSpaces[x, y];
        if (otherPiece != null)
        {
            if (otherPiece.Color != piece.Color)
            {
                if (moveCheckCounter == 0)
                {
                    if (piece.MoveDoesNotPutKingInCheck(startX, startY, x, y, boardSpaces))
                    {
                        validMoves.Add("Attack" + v, new int[] { x, y });
                    }
                }
            }
        }
        else
        {
            if (moveCheckCounter == 0)
            {
                if (piece.MoveDoesNotPutKingInCheck(startX, startY, x, y, boardSpaces))
                {
                    validMoves.Add("Move" + v, new int[] { x, y });
                }
            }
        }
    }
}
