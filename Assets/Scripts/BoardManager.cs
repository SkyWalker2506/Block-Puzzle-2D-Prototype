using Puzzle2DSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] int minBoardSize=4;
    [SerializeField] int maxBoardSize = 6;
    [SerializeField] int minPuzzlePieceCount=5;
    [SerializeField] int maxPuzzlePieceCount=8;
    [SerializeField] Material materialPrefab;
    [SerializeField] bool useLineRenderer;
    PuzzleDataCreator puzzleDataCreator;
    PuzzleDataVisualizer puzzleDataVisualizer;
    PuzzleData puzzleData;
    GameObject puzzleBoard;
    GameObject[] puzzlePieces=new GameObject[0];


    private void Start()
    {
        SetBoardSettings();
        CreateRandomPuzzle();
    }

    [ContextMenu("Assign Changed Board Settings")]
    private void SetBoardSettings()
    {
        puzzleDataCreator = new PuzzleDataCreator(minBoardSize, maxBoardSize, minPuzzlePieceCount, maxPuzzlePieceCount);
        puzzleData = puzzleDataCreator.GetRandomPuzzleData();
        if (useLineRenderer)
            puzzleDataVisualizer = new PuzzleDataVisualizerWithLineRenderers(puzzleData, materialPrefab);
        else
            puzzleDataVisualizer = new PuzzleDataVisualizerWithMeshRenderer(puzzleData, materialPrefab);
    }

    [ContextMenu("Create Random Board")]
    void CreateRandomPuzzle()
    {
        if (puzzleBoard)
            Destroy(puzzleBoard);
        if (puzzlePieces.Length>0)
            {
                foreach (var piece in puzzlePieces)
                {
                    Destroy(piece);
                }
            }
        puzzleBoard =puzzleDataVisualizer.CreateBoardVisual();
        puzzlePieces = puzzleDataVisualizer.CreatePuzzlePiecesVisual();
        SetCamera();
    }

    private void SetCamera()
    {
        Camera.main.orthographicSize = puzzleData.PuzzleBoard.BoardSize;
        Camera.main.transform.position = new Vector2(.5f, .25f) * puzzleData.PuzzleBoard.BoardSize;
        Camera.main.transform.position -= Vector3.forward;
    }
}
