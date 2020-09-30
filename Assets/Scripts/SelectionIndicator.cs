using UnityEngine;

public class SelectionIndicator : MonoBehaviour
{
    Chessboard chessboard;
    // Start is called before the first frame update
    void Start()
    {
        chessboard = FindObjectOfType<Chessboard>();
    }

    // Update is called once per frame
    void Update()
    {
        Piece p = chessboard.GetSelectedPiece();
        if (p != null)
        {
            transform.position = p.transform.position;
        }
        else
        {
            transform.position = new Vector3(0, -1, 0);
        }
    }
}
