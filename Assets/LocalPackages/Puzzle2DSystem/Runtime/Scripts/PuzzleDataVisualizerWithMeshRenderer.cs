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


        public override void SetPuzzleData(PuzzleData data)
        {
            puzzleData = data;
        }


        public override GameObject CreateBoardVisual()
        {
            var board = new GameObject("Board ", typeof(MeshFilter), typeof(MeshRenderer));
            board.transform.position = new Vector3(0, 0, .25f);

            Vector3[] vertices = new Vector3[4];
            int[] triangles = new int[6];

            vertices[0] = new Vector2(-0.025f, -0.025f) * puzzleData.PuzzleBoard.BoardSize;
            vertices[1] = new Vector2(-0.025f, 1.025f) * puzzleData.PuzzleBoard.BoardSize; 
            vertices[2] = new Vector2(1.025f, 1.025f) * puzzleData.PuzzleBoard.BoardSize; 
            vertices[3] = new Vector2(1.025f, -0.025f) * puzzleData.PuzzleBoard.BoardSize; 

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 3;
            triangles[3] = 3;
            triangles[4] = 1;
            triangles[5] = 2;

            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;

            board.GetComponent<MeshFilter>().mesh = mesh;

            var material = Object.Instantiate(materialPrefab);
            material.color = new Color(.25f, .25f, .25f);

            board.GetComponent<MeshRenderer>().material = material;

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
            var piece = new GameObject("Piece ",typeof(MeshFilter), typeof(MeshRenderer));

            piece.transform.position = puzzlePiece.Center;


            var pointCount = puzzlePiece.TriangleDatas.Count * 3;
            Vector3[] vertices = new Vector3[pointCount];
            //Vector2[] uv = new Vector2[pointCount];
            int[] triangles = new int[pointCount];

            for (int i = 0; i < puzzlePiece.TriangleDatas.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    vertices[i * 3 + j] = puzzlePiece.TriangleDatas[i].Points[j]- puzzlePiece.Center;
                    //uv[i * 3 + j] = puzzlePiece.TriangleDatas[i].Points[j].normalized;
                    triangles[i * 3 + j] = i * 3 + j;
                }
            }

            var mesh = new Mesh();
            mesh.vertices = vertices;
            //mesh.uv = uv;
            mesh.triangles =triangles;

            piece.GetComponent<MeshFilter>().mesh = mesh;

            var material = Object.Instantiate(materialPrefab);
            material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            piece.GetComponent<MeshRenderer>().material = material;

            return piece;
        }

    }
}