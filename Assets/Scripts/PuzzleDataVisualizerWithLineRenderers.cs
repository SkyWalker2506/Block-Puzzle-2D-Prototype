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

        public override void CreateBoardVisual()
        {
            var board = new GameObject("Board");
            var renderer = board.AddComponent<LineRenderer>();
            var material = GameObject.Instantiate(materialPrefab);
            renderer.material = material;
            renderer.startWidth = .2f;
            renderer.endWidth = .2f;
            renderer.positionCount = 5;
            renderer.SetPosition(0, Vector2.zero);
            renderer.SetPosition(1, puzzleData.PuzzleBoard.BoardSize * Vector2.up);
            renderer.SetPosition(2, puzzleData.PuzzleBoard.BoardSize * (Vector2.up + Vector2.right));
            renderer.SetPosition(3, puzzleData.PuzzleBoard.BoardSize * Vector2.right);
            renderer.SetPosition(4, Vector2.zero);
        }

        public override void CreatePuzzlePiecesVisual()
        {
            puzzleData.PuzzlePieces.ForEach(CreatePuzzlePiece);
        }

        void CreatePuzzlePiece(PuzzlePiece puzzlePiece)
        {
            var piece = new GameObject("piece");
            var material = Object.Instantiate(materialPrefab);
            material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            for (int i = 0; i < puzzlePiece.TriangleDatas.Count; i++)
            {
                CreateTriangle(puzzlePiece.TriangleDatas[i], material).transform.SetParent(piece.transform);
            }
        }

        GameObject CreateTriangle(TriangleData triangleData, Material material)
        {
            var triangle = new GameObject("puzzlePieceTriangle");
            var renderer = triangle.AddComponent<LineRenderer>();
            renderer.material = material;
            renderer.startWidth = .2f;
            renderer.endWidth = .2f;
            renderer.positionCount = 4;
            for (int i = 0; i < 4; i++)
            {
                renderer.SetPosition(i, triangleData.Points[i % 3]);
            }
            return triangle;
        }
    }
}