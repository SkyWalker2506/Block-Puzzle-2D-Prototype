using UnityEngine;

namespace Puzzle2DSystem
{
    public class PuzzleDataVisualizerWithLineRenderers : PuzzleDataVisualizer
    {
        PuzzleData puzzleData;
        Material materialPrefab;

        public PuzzleDataVisualizerWithLineRenderers(PuzzleData data,Material material)
        {
            puzzleData = data;
            materialPrefab = material;
        }

        public override GameObject CreateBoardVisual()
        {
            var board = new GameObject("Board");
            board.transform.position = new Vector3(0, 0, .25f);
            var renderer = board.AddComponent<LineRenderer>();
            renderer.useWorldSpace = false;
            var material = Object.Instantiate(materialPrefab);
            renderer.material = material;
            renderer.startWidth = .25f;
            renderer.endWidth = .25f;
            renderer.positionCount = 5;
            renderer.SetPosition(0, Vector2.zero);
            renderer.SetPosition(1, puzzleData.PuzzleBoard.BoardSize * Vector2.up);
            renderer.SetPosition(2, puzzleData.PuzzleBoard.BoardSize * (Vector2.up + Vector2.right));
            renderer.SetPosition(3, puzzleData.PuzzleBoard.BoardSize * Vector2.right);
            renderer.SetPosition(4, Vector2.zero);
            return board;
        }

        public override GameObject[] CreatePuzzlePiecesVisual()
        {
            var pieceCount = puzzleData.PuzzlePieces.Count;

            PuzzlePieces = new GameObject[pieceCount];

            for (int i = 0; i < pieceCount; i++)
            {
                PuzzlePieces[i] = CreatePuzzlePiece(puzzleData.PuzzlePieces[i]);
            }
            return PuzzlePieces;
        }

        GameObject CreatePuzzlePiece(PuzzlePiece puzzlePiece)
        {
            var piece = new GameObject("piece");
            piece.transform.position = puzzlePiece.Center;
            var material = Object.Instantiate(materialPrefab);
            material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            for (int i = 0; i < puzzlePiece.TriangleDatas.Count; i++)
            {
                CreateTriangle(puzzlePiece.TriangleDatas[i], material).transform.SetParent(piece.transform);
            }
            return piece;
        }

        GameObject CreateTriangle(TriangleData triangleData, Material material)
        {
            var triangle = new GameObject("puzzlePieceTriangle");
            var renderer = triangle.AddComponent<LineRenderer>();
            renderer.useWorldSpace = false;
            renderer.material = material;
            renderer.startWidth = .1f;
            renderer.endWidth = .1f;
            renderer.positionCount = 4;
            for (int i = 0; i < 4; i++)
            {
                renderer.SetPosition(i, triangleData.Points[i % 3]);
            }
            return triangle;
        }
    }
}