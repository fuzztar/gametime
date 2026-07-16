using System.Collections.Generic;
using UnityEngine;

public class JigsawManager : MonoBehaviour
{
    [Header("Puzzle UI")]
    [SerializeField] private GameObject puzzlePanel;


    [Header("Puzzle Layout")]
    [SerializeField] private Transform pieceContainer;
    [SerializeField] private Transform snapPointParent;


    [Header("Random Scatter")]
    [SerializeField] private float scatterX = 250f;
    [SerializeField] private float scatterY = 150f;


    [Header("Player References")]
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private ObjectHighlighter objectHighlighter;


    [Header("Door")]
    [SerializeField] private DoorInteraction linkedDoor;



    private Dictionary<int, SnapPoint> snapPoints =
        new Dictionary<int, SnapPoint>();

    private List<JigsawPiece> pieces =
        new List<JigsawPiece>();


    private int completedPieces = 0;


    public bool PuzzleComplete { get; private set; }



    private void Awake()
    {
        FindSnapPoints();
        FindPieces();


        if (puzzlePanel != null)
        {
            puzzlePanel.SetActive(false);
        }
    }



    private void FindSnapPoints()
    {
        snapPoints.Clear();


        SnapPoint[] points =
            snapPointParent.GetComponentsInChildren<SnapPoint>();


        foreach (SnapPoint point in points)
        {
            if (!snapPoints.ContainsKey(point.SnapID))
            {
                snapPoints.Add(point.SnapID, point);
            }
            else
            {
                Debug.LogWarning(
                    "Duplicate SnapPoint ID: " + point.SnapID
                );
            }
        }


        Debug.Log(
            "Snap Points Loaded: "
            + snapPoints.Count
        );
    }



    private void FindPieces()
    {
        pieces.Clear();


        JigsawPiece[] foundPieces =
            pieceContainer.GetComponentsInChildren<JigsawPiece>();


        foreach (JigsawPiece piece in foundPieces)
        {
            RegisterPiece(piece);
        }


        Debug.Log(
            "Pieces Loaded: "
            + pieces.Count
        );
    }



    public void RegisterPiece(JigsawPiece piece)
    {
        if (!pieces.Contains(piece))
        {
            pieces.Add(piece);
        }
    }



    public SnapPoint GetSnapPoint(int id)
    {
        if (snapPoints.TryGetValue(id, out SnapPoint point))
        {
            return point;
        }


        Debug.LogWarning(
            "No SnapPoint found with ID: "
            + id
        );


        return null;
    }



    public void OpenPuzzle()
    {
        if (PuzzleComplete)
            return;


        puzzlePanel.SetActive(true);


        ScatterPieces();


        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


        if (mouseLook != null)
            mouseLook.enabled = false;


        if (playerMovement != null)
            playerMovement.enabled = false;


        if (objectHighlighter != null)
            objectHighlighter.uiOpen = true;
    }



    private void ScatterPieces()
    {
        foreach (JigsawPiece piece in pieces)
        {
            RectTransform rect =
                piece.GetComponent<RectTransform>();


            float randomX =
                Random.Range(-scatterX, scatterX);


            float randomY =
                Random.Range(-scatterY, scatterY);


            rect.anchoredPosition =
                new Vector2(randomX, randomY);
        }
    }



    public void PieceCompleted()
    {
        completedPieces++;


        Debug.Log(
            "Puzzle Progress: "
            + completedPieces
            + "/"
            + pieces.Count
        );


        if (completedPieces >= pieces.Count)
        {
            CompletePuzzle();
        }
    }



    private void CompletePuzzle()
    {
        PuzzleComplete = true;


        Debug.Log(
            "JIGSAW PUZZLE COMPLETE!"
        );


        ClosePuzzle();


        if (linkedDoor != null)
        {
            linkedDoor.UnlockDoor();
        }
    }



    public void ClosePuzzle()
    {
        puzzlePanel.SetActive(false);


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        if (mouseLook != null)
            mouseLook.enabled = true;


        if (playerMovement != null)
            playerMovement.enabled = true;


        if (objectHighlighter != null)
            objectHighlighter.uiOpen = false;
    }
}