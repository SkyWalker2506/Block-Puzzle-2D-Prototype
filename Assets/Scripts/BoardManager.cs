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

    PuzzleDataCreator puzzleDataCreator;
    PuzzleDataVisualizer puzzleDataVisualizer;
    PuzzleData puzzleData;

    private void Awake()
    {
        puzzleDataCreator = new PuzzleDataCreator(minBoardSize, maxBoardSize,minPuzzlePieceCount,maxPuzzlePieceCount);
        puzzleData = puzzleDataCreator.GetRandomPuzzleData();
        puzzleDataVisualizer = new PuzzleDataVisualizerWithMeshRenderer(puzzleData, materialPrefab);
        Camera.main.orthographicSize = puzzleData.PuzzleBoard.BoardSize ;
        Camera.main.transform.position =  new Vector2(.5f,.25f) * puzzleData.PuzzleBoard.BoardSize;
        Camera.main.transform.position -= Vector3.forward;
        puzzleDataVisualizer.CreateBoardVisual();
        puzzleDataVisualizer.CreatePuzzlePiecesVisual();
    }

}
