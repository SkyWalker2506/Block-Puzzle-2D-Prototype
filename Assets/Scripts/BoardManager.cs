using Puzzle2DSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    int minBoardSize=4;
    int maxBoardSize=6;
    int minPuzzlePieceCount;
    int maxPuzzlePieceCount;

    PuzzleDataCreator puzzleDataCreator;

    private void Awake()
    {
        puzzleDataCreator = new PuzzleDataCreator();
        puzzleDataCreator.GetRandomPuzzleData();
    }

}
