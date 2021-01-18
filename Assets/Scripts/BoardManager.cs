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
    PuzzleData puzzleData;

    private void Awake()
    {
        puzzleDataCreator = new PuzzleDataCreator(minBoardSize, maxBoardSize,minPuzzlePieceCount,maxPuzzlePieceCount);
        puzzleData = puzzleDataCreator.GetRandomPuzzleData();
        Camera.main.orthographicSize = puzzleData.PuzzleBoard.BoardSize ;
        Camera.main.transform.position =  Vector2.one * puzzleData.PuzzleBoard.BoardSize*.5f;
        Camera.main.transform.position -= Vector3.forward;
        CreatePuzzlePiecesVisual();
        CreateBoardVisual();
    }

    void CreateBoardVisual()
    {
        print(puzzleData.PuzzlePieces.Count);
        var board = new GameObject("Board");
        var renderer = board.AddComponent<LineRenderer>();
        var material = Instantiate(materialPrefab);
        renderer.material = material;
        renderer.startWidth = .2f;
        renderer.endWidth = .2f;
        renderer.positionCount = 5;
        renderer.SetPosition(0, Vector2.zero);
        renderer.SetPosition(1, puzzleData.PuzzleBoard.BoardSize * Vector2.up);
        renderer.SetPosition(2, puzzleData.PuzzleBoard.BoardSize * (Vector2.up+ Vector2.right));
        renderer.SetPosition(3, puzzleData.PuzzleBoard.BoardSize * Vector2.right);
        renderer.SetPosition(4, Vector2.zero);
    }

    void CreatePuzzlePiecesVisual()
    {
        print(puzzleData.PuzzlePieces.Count);
        puzzleData.PuzzlePieces.ForEach(CreatePuzzlePiece);
    }


    void CreatePuzzlePiece(PuzzlePiece puzzlePiece)
    {
        print(puzzlePiece.TriangleDatas.Length);
        var piece = new GameObject("piece");
        var material = Instantiate(materialPrefab);
        material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        for (int i = 0; i < puzzlePiece.TriangleDatas.Length; i++)
        {
             CreateTriangle(puzzlePiece.TriangleDatas[i], material).transform.SetParent(piece.transform);
        }
    }

    GameObject CreateTriangle(TriangleData triangleData,Material material)
    {
        var triangle = new GameObject("puzzlePieceTriangle");
        var renderer = triangle.AddComponent<LineRenderer>();
        renderer.material = material;
        renderer.startWidth= .2f;
        renderer.endWidth= .2f;
        renderer.positionCount = 4;
        for (int i = 0; i < 4; i++)
        {
            renderer.SetPosition(i, triangleData.Points[i%3]);
        }
        return triangle;
    }
}
