using UnityEngine;

namespace Puzzle2DSystem
{
    public class PuzzleDataVisualizerWithMeshRenderer : PuzzleDataVisualizer
    {

        PuzzleData puzzleData;
        Material materialPrefab;


        public PuzzleDataVisualizerWithMeshRenderer(PuzzleData data, Material material)
        {
            puzzleData = data;
            materialPrefab = material;
        }


        public override void CreateBoardVisual()
        {
            var board = new GameObject("Board ", typeof(MeshFilter), typeof(MeshRenderer));

            Vector3[] vertices = new Vector3[0];
            Vector2[] uv = new Vector2[0];
            int[] triangles = new int[0];

            //Create Mesh

            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            board.GetComponent<MeshFilter>().mesh = mesh;

            var material = Object.Instantiate(materialPrefab);
            material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            board.GetComponent<MeshRenderer>().material = material;

        }

        public override void CreatePuzzlePiecesVisual()
        {
            var pieceCount = puzzleData.PuzzlePieces.Count;
            PuzzlePieces = new GameObject[pieceCount];
            for (int i = 0; i < pieceCount; i++)
            {
                PuzzlePieces[i] = CreatePuzzlePiece(puzzleData.PuzzlePieces[i]);
            }
        }


        GameObject CreatePuzzlePiece(PuzzlePiece puzzlePiece)
        {
            var piece = new GameObject("Piece ",typeof(MeshFilter), typeof(MeshRenderer));

            var pointCount = puzzlePiece.TriangleDatas.Count * 3;
            Vector3[] vertices = new Vector3[pointCount];
            Vector2[] uv = new Vector2[pointCount];
            int[] triangles = new int[pointCount];

            for (int i = 0; i < puzzlePiece.TriangleDatas.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    vertices[i * 3 + j] = puzzlePiece.TriangleDatas[i].Points[j];
                    uv[i * 3 + j] = puzzlePiece.TriangleDatas[i].Points[j].normalized;
                    triangles[i * 3 + j] = i * 3 + j;
                }
            }

            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles =triangles;

            piece.GetComponent<MeshFilter>().mesh = mesh;

            var material = Object.Instantiate(materialPrefab);
            material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            piece.GetComponent<MeshRenderer>().material = material;

            return piece;
        }

    }
}