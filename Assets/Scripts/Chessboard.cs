using System;
using System.Collections.Generic;
using UnityEngine;

public class Chessboard : MonoBehaviour
{
    public Piece[,] pieces = new Piece[8, 8];
    public GameObject blackPawnPrefab;
    public GameObject blackRookPrefab;
    public GameObject blackKnightPrefab;
    public GameObject blackBishopPrefab;
    public GameObject blackQueenPrefab;
    public GameObject blackKingPrefab;
    public GameObject whitePawnPrefab;
    public GameObject whiteRookPrefab;
    public GameObject whiteKnightPrefab;
    public GameObject whiteBishopPrefab;
    public GameObject whiteQueenPrefab;
    public GameObject whiteKingPrefab;
    public GameObject selectionBox;
    public GameObject movementBox;
    public Vector3 boardOffset = new Vector3(-4.0f, 0.4f, -4.0f);
    public Vector3 pieceOffset = new Vector3(0.5f, 0f, 0.5f);
    public string playerTurn;
    private int startx;
    private int starty;
    private Piece selectedPiece;
    private GameObject legalSpace;
    private Vector2 mouseOver;
    private List<GameObject> legalSpaceObjectList = new List<GameObject>();
    private Dictionary<string, int[]> legalSpacesDictionary;
    private Dictionary<string, int[]> newSpacesDictionary;

    //Create pieces one by one
    private void GenerateBoard()
    {
        //Generate white team
        for(int row = 0; row  < 2; row++)
        {
            for(int col = 0; col < 8; col++)
            {
                GeneratePiece("white", col, row);
            }
        }
        //Generate black team
        for (int row = 7; row > 5; row--)
        {
            for (int col = 0; col < 8; col++)
            {
                GeneratePiece("black", col, row);
            }
        }
    }

    private void GeneratePiece(string color, int x, int y)
    {
        bool isWhite = color == "white";
        GameObject gameObject;
        if (y == 6 || y == 1) { gameObject = Instantiate((isWhite) ? whitePawnPrefab : blackPawnPrefab) as GameObject; }
        else
        {
            if (x == 0 || x == 7)      { gameObject = Instantiate((isWhite) ? whiteRookPrefab : blackRookPrefab) as GameObject; }
            else if (x == 1 || x == 6) { gameObject = Instantiate((isWhite) ? whiteKnightPrefab : blackKnightPrefab) as GameObject; }
            else if (x == 2 || x == 5) { gameObject = Instantiate((isWhite) ? whiteBishopPrefab : blackBishopPrefab) as GameObject; }
            else if (x == 3)           { gameObject = Instantiate((isWhite) ? whiteQueenPrefab : blackQueenPrefab) as GameObject; }
            else                       { gameObject = Instantiate((isWhite) ? whiteKingPrefab : blackKingPrefab) as GameObject; }
        }
        gameObject.transform.SetParent(transform);
        Piece p = gameObject.GetComponent<Piece>();
        pieces[x, y] = p;
        MovePiece(p, x, y);
    }

    private void MovePiece(Piece p, int x, int y, Piece enemyPiece = null)
    {
        if (enemyPiece != null && enemyPiece.Color != p.Color)
        {
            pieces[x, y] = null;
            Destroy(enemyPiece.gameObject);
        }
        p.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset + pieceOffset;
    }

    internal bool CheckForCheck(int x, int y)
    {
        Piece piece = pieces[x, y];
        newSpacesDictionary = piece.FindLegalSpaces(x, y, pieces, false);
        if (playerTurn == "White")
        {
            foreach (var item in newSpacesDictionary)
            {
                if (pieces[item.Value[0], item.Value[1]] != null)
                {
                    Piece otherpiece = pieces[item.Value[0], item.Value[1]];
                    if (otherpiece.PieceName == "King" && otherpiece.Color != piece.Color)
                    {
                        otherpiece.SetKingStatus(true);
                        return true;
                    }
                }
            }
            SetKingAsSafe(piece);
        }
        else
        {
            foreach (var item in newSpacesDictionary)
            {
                if (pieces[item.Value[0], item.Value[1]] != null)
                {
                    Piece otherpiece = pieces[item.Value[0], item.Value[1]];
                    if (otherpiece.PieceName == "King" && otherpiece.Color != piece.Color)
                    {
                        otherpiece.SetKingStatus(true);
                        return true;
                    }
                }
            }
            SetKingAsSafe(piece);
        }
        return false;
    }

    private void SetKingAsSafe(Piece piece)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Piece king = pieces[i, j];
                if (king != null && king.PieceName == "King" && king.Color == piece.Color)
                {
                    king.SetKingStatus(false);
                }
            }
        }
    }

    private bool CheckForCheckMate()
    {
        int validMovesCount = 0;
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if(pieces[i,j] != null && pieces[i,j].Color != playerTurn)
                {
                    Dictionary<string, int[]> validMoves = pieces[i,j].FindLegalSpaces(i, j, pieces, false, 0);
                    validMovesCount += validMoves.Count;
                }
            }
        }
        if(validMovesCount == 0)
        {
            return true;
        }
        return false;
    }

    private bool CheckForStalemate()
    {
        List<Piece> remainingPieces = new List<Piece>();
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if(pieces[i,j] != null)
                {
                    remainingPieces.Add(pieces[i, j]);
                }
            }
        }
        if(remainingPieces.Count == 2 && OnlyKingsRemain(remainingPieces))
        {
            return true;
        }
        if(playerTurn == "White")
        {
            if (KingHasNoMovesAndNotInCheck("Black"))
            {
                return true;
            }
        }
        else
        {
            if (KingHasNoMovesAndNotInCheck("White"))
            {
                return true;
            }
        }
        return false;
    }

    private bool KingHasNoMovesAndNotInCheck(string color)
    {
        Piece king = null;
        int kingX = 0;
        int kingY = 0;
        int pieceCount = 0;
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if(pieces[i,j] != null && pieces[i,j].Color == color)
                {
                    if (pieces[i, j].PieceName == "King")
                    {
                        king = pieces[i, j];
                        kingX = i;
                        kingY = j;
                    }
                    pieceCount++;
                }
            }
        }
        legalSpacesDictionary = king.FindLegalSpaces(kingX, kingY, pieces, false);
        if (legalSpacesDictionary.Count == 0 && king.KingIsSafe(king, pieces, kingX, kingY) && pieceCount == 1)
        {
            return true;
        }
        return false;
    }

    private bool OnlyKingsRemain(List<Piece> remainingPieces)
    {
        foreach (Piece piece in remainingPieces)
        {
            if(piece.PieceName != "King")
            {
                return false;
            }
        }
        return true;
    }

    private void UpdateMouseOver()
    {
        if (!Camera.main) { return; }
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Board")))
        {
            mouseOver.x = (int)(hit.point.x - boardOffset.x);
            mouseOver.y = (int)(hit.point.z - boardOffset.z);
        }
        else
        {
            mouseOver.x = -1;
            mouseOver.y = -1;
        }
    }

    public Piece GetSelectedPiece()
    {
        return selectedPiece;
    }

    private void SelectPiece(int x, int y)
    {
        if (x < 0 || x >= pieces.Length || y < 0 || y >= pieces.Length)
        {
            return;
        }
        Piece p = pieces[x, y];
        if (p != null)
        {
            if (p.Color == playerTurn)
            {
                selectedPiece = p;
                startx = x;
                starty = y;
                if (legalSpaceObjectList != null || legalSpaceObjectList.Count > 0)
                {
                    DestroyLegalSpaces();
                }
                legalSpacesDictionary = selectedPiece.FindLegalSpaces(x, y, pieces, false, 0);
                foreach (var item in legalSpacesDictionary)
                {
                    legalSpace = Instantiate(movementBox) as GameObject;
                    legalSpace.gameObject.GetComponent<MovementSpace>().SetMaterial(item.Key);
                    legalSpace.name = item.Value[0].ToString() + item.Value[1].ToString();
                    legalSpace.transform.position = (Vector3.right * item.Value[0])
                        + (Vector3.up * .01f)
                        + (Vector3.forward * item.Value[1])
                        + boardOffset + pieceOffset;
                    legalSpaceObjectList.Add(legalSpace);
                }
            }
            else
            {
                if (selectedPiece != null && ValidSpace(x, y, legalSpacesDictionary)) { CommitToMovement(x, y, p); }
            }
        }
        else if(p == null && selectedPiece != null && ValidSpace(x, y, legalSpacesDictionary))
        {
            if (MoveIsCastle(x, y, legalSpacesDictionary))      { Castle(startx, x); }
            if (MoveIsEnPassant(x, y, legalSpacesDictionary))   { EnPassant(selectedPiece, x, y); }
            CommitToMovement(x, y, p);
        }
    }

    private void EnPassant(Piece selectedPiece, int x, int y)
    {
        if (selectedPiece.Color == "White")
        {
            if (y - 1 != 5)
            {
                Piece otherPiece = pieces[x, y - 1];
                pieces[x, y - 1] = null;
                Destroy(otherPiece.gameObject);
            }
        }
        else
        {
            if (y + 1 != 1)
            {
                Piece otherPiece = pieces[x, y + 1];
                pieces[x, y + 1] = null;
                Destroy(otherPiece.gameObject);
            }
        }
    }

    private bool MoveIsEnPassant(int x, int y, Dictionary<string, int[]> legalSpacesDictionary)
    {
        if (legalSpacesDictionary.Count == 0) { return false; }
        foreach (var entry in legalSpacesDictionary)
        {
            int[] entryValues = entry.Value;
            if (entryValues[0] == x && entryValues[1] == y && entry.Key.Contains("EnPassant")) { return true; }
        }
        return false;
    }

    private void CommitToMovement(int x, int y, Piece p)
    {
        MovePiece(selectedPiece, x, y, p);
        DestroyLegalSpaces();
        pieces[x, y] = selectedPiece;
        pieces[startx, starty] = null;
        selectedPiece.ChangeMoveStatus();
        if (selectedPiece.PieceName == "Pawn")
        {
            selectedPiece.CheckForPromotion(y);
            selectedPiece.IncreaseMoveCounter();
        }
        selectedPiece = null;
        if (CheckForCheck(x, y))
        {
            if (CheckForCheckMate())
            {
                Debug.Log("Checkmate");
            }
            Debug.Log("check");
        }
        if (CheckForStalemate())
        {
            Debug.Log("Stalemate");
        }
        playerTurn = (playerTurn == "White") ? "Black" : "White";
    }

    private void Castle(int startx, int x)
    {
        if (selectedPiece.Color == "White")
        {
            if (startx < x) { CastleRook(0, 7); }
            else            { CastleRook(0, 0); }
        }
        else
        {
            if (startx < x) { CastleRook(7, 7); }
            else            { CastleRook(7, 0); }
        }
    }

    private void CastleRook(int row, int col)
    {
        Piece rookPiece = pieces[col, row];
        if (col == 7)
        {
            MovePiece(rookPiece, col - 2, row);
            pieces[col - 2, row] = rookPiece;
        }
        else
        {
            MovePiece(rookPiece, col + 3, row);
            pieces[col + 3, row] = rookPiece;
        }
        pieces[col, row] = null;
        rookPiece.ChangeMoveStatus();
    }

    private bool MoveIsCastle(int x, int y, Dictionary<string, int[]> legalSpacesDictionary)
    {
        if (legalSpacesDictionary.Count == 0) { return false; }
        foreach (var entry in legalSpacesDictionary)
        {
            int[] entryValues = entry.Value;
            if (entryValues[0] == x && entryValues[1] == y && entry.Key.Contains("Castle")) { return true; }
        }
        return false;
    }

    private bool ValidSpace(int x, int y, Dictionary<string, int[]> legalSpacesDictionary)
    {
        if (legalSpacesDictionary.Count == 0) { return false; }
        foreach (var value in legalSpacesDictionary.Values)
        {
            if (value[0] == x && value[1] == y) { return true; }
        }
        return false;
    }

    private void DestroyLegalSpaces()
    {
        foreach (GameObject obj in legalSpaceObjectList)
        {
            if (obj != null) { Destroy(obj); }
        }
    }

    void Start()
    {
        GenerateBoard();
        GenerateSelectionBox();
        playerTurn = "White";
    }

    private void GenerateSelectionBox()
    {
        selectionBox = Instantiate(selectionBox) as GameObject;
    }

    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            UpdateMouseOver();
            int x = (int)mouseOver.x;
            int y = (int)mouseOver.y;
            if (playerTurn == "White")
            {
                if (Input.GetMouseButtonDown(0)) { SelectPiece(x, y); }
            }
            else
            {
                if (Input.GetMouseButtonDown(0)) { SelectPiece(x, y); }
            }
        }
    }
}
