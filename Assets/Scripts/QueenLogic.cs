using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenLogic : MonoBehaviour
{
    private int startX;
    private int startY;
    private int moveCheckCounter;
    internal Dictionary<string, int[]> GetQueenMovementSpaces(Piece piece, int x, int y, Piece[,] p, bool justChecking, int moveCheckCounter = 0)
    {
        Dictionary<string, int[]> validMoves = new Dictionary<string, int[]>();
        startX = x;
        startY = y;
        int newX;
        int newY;
        bool canStillMove;
        for (int i = 0; i < 8; i++)
        {
            if (piece.InBoardDimensions(x, y, i, i, p))
            {
                newX = x + i;
                newY = y + i;
                if (moveCheckCounter != 0)
                {
                    canStillMove = GetMoves(piece, p, ref validMoves, newX, newY, "0" + i.ToString(), justChecking);
                    if (!canStillMove) { break; }
                }
                else
                {
                    canStillMove = FindValidMoves(piece, p, ref validMoves, newX, newY, "0" + i.ToString(), justChecking);
                    if (!canStillMove) { break; }
                }
            }
        }
        for (int i = 0; i < 8; i++)
        {
            if (piece.InBoardDimensions(x, y, i, -1 * i, p))
            {
                newX = x + i;
                newY = y - i;
                if (moveCheckCounter != 0)
                {
                    canStillMove = GetMoves(piece, p, ref validMoves, newX, newY, "1" + i.ToString(), justChecking);
                    if (!canStillMove) { break; }
                }
                else
                {
                    canStillMove = FindValidMoves(piece, p, ref validMoves, newX, newY, "1" + i.ToString(), justChecking);
                    if (!canStillMove) { break; }
                }
            }
        }
        for (int i = 0; i < 8; i++)
        {
            if (piece.InBoardDimensions(x, y, -1 * i, -1 * i, p))
            {
                newX = x - i;
                newY = y - i;
                if (moveCheckCounter != 0)
                {
                    canStillMove = GetMoves(piece, p, ref validMoves, newX, newY, "2" + i.ToString(), justChecking);
                    if (!canStillMove) { break; }
                }
                else
                {
                    canStillMove = FindValidMoves(piece, p, ref validMoves, newX, newY, "2" + i.ToString(), justChecking);
                    if (!canStillMove) { break; }
                }
            }
        }
        for (int i = 0; i < 8; i++)
        {
            if (piece.InBoardDimensions(x, y, -1 * i, i, p))
            {
                newX = x - i;
                newY = y + i;
                if (moveCheckCounter != 0)
                {
                    canStillMove = GetMoves(piece, p, ref validMoves, newX, newY, "3" + i.ToString(), justChecking);
                    if (!canStillMove) { break; }
                }
                else
                {
                    canStillMove = FindValidMoves(piece, p, ref validMoves, newX, newY, "3" + i.ToString(), justChecking);
                    if (!canStillMove) { break; }
                }
            }
        }
        for (int i = 0; i < 8; i++)
        {
            if (piece.InBoardDimensions(x, y, 0, i, p))
            {
                newX = x;
                newY = y + i;
                if (moveCheckCounter != 0)
                {
                    canStillMove = GetMoves(piece, p, ref validMoves, newX, newY, "4" + i.ToString(), justChecking);
                    if (!canStillMove) { break; }
                }
                else
                {
                    canStillMove = FindValidMoves(piece, p, ref validMoves, newX, newY, "4" + i.ToString(), justChecking);
                    if (!canStillMove) { break; }
                }
            }
        }
        for (int i = 0; i < 8; i++)
        {
            if (piece.InBoardDimensions(x, y, i, 0, p))
            {
                newX = x + i;
                newY = y;
                if (moveCheckCounter != 0)
                {
                    canStillMove = GetMoves(piece, p, ref validMoves, newX, newY, "5" + i.ToString(), justChecking);
                    if (!canStillMove) { break; }
                }
                else
                {
                    canStillMove = FindValidMoves(piece, p, ref validMoves, newX, newY, "5" + i.ToString(), justChecking);
                    if (!canStillMove) { break; }
                }
            }
        }
        for (int i = 0; i < 8; i++)
        {
            if (piece.InBoardDimensions(x, y, 0, -1 * i, p))
            {
                newX = x;
                newY = y - i;
                if (moveCheckCounter != 0)
                {
                    canStillMove = GetMoves(piece, p, ref validMoves, newX, newY, "6" + i.ToString(), justChecking);
                    if (!canStillMove) { break; }
                }
                else
                {
                    canStillMove = FindValidMoves(piece, p, ref validMoves, newX, newY, "6" + i.ToString(), justChecking);
                    if (!canStillMove) { break; }
                }
            }
        }
        for (int i = 0; i < 8; i++)
        {
            if (piece.InBoardDimensions(x, y, -1 * i, 0, p))
            {
                newX = x - i;
                newY = y;
                if (moveCheckCounter != 0)
                {
                    canStillMove = GetMoves(piece, p, ref validMoves, newX, newY, "7" + i.ToString(), justChecking);
                    if (!canStillMove) { break; }
                }
                else
                {
                    canStillMove = FindValidMoves(piece, p, ref validMoves, newX, newY, "7" + i.ToString(), justChecking);
                    if (!canStillMove) { break; }
                }
            }
        }
        return validMoves;
    }
    private bool GetMoves(Piece piece, Piece[,] boardSpaces, ref Dictionary<string, int[]> validMoves, int x, int y, string v, bool canContinue)
    {
        Piece otherPiece = boardSpaces[x, y];
        if (otherPiece == piece)
        {
            return true;
        }
        if (otherPiece != null)
        {
            if (otherPiece.Color != piece.Color)
            {
                validMoves.Add("Attack" + v, new int[] { x, y });
                if (otherPiece.PieceName == "King" && canContinue)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        else
        {
            validMoves.Add("Move" + v, new int[] { x, y });
            return true;
        }
    }
    private bool FindValidMoves(Piece piece, Piece[,] boardSpaces, ref Dictionary<string, int[]> validMoves, int x, int y, string v, bool canContinue)
    {
        Piece otherPiece = boardSpaces[x, y];
        if (otherPiece == piece)
        {
            return true;
        }
        if (otherPiece != null)
        {
            if (otherPiece.Color != piece.Color)
            {
                if (moveCheckCounter == 0)
                {
                    if (piece.MoveDoesNotPutKingInCheck(startX, startY, x, y, boardSpaces) && !canContinue)
                    {
                        validMoves.Add("Attack" + v, new int[] { x, y });
                        if (otherPiece.PieceName == "King" && canContinue)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            return false;
        }
        else
        {
            if (moveCheckCounter == 0)
            {
                if (piece.MoveDoesNotPutKingInCheck(startX, startY, x, y, boardSpaces) && !canContinue)
                {
                    validMoves.Add("Move" + v, new int[] { x, y });
                }
            }
            return true;
        }
    }
}
