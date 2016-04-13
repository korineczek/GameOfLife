using UnityEngine;
using System.Collections;

public class Board : Singleton<Board>
{
    public Transform Tile;
    public int BoardSize = 16;

    // Data storage
    public MeshRenderer[,] boardTiles;
    public int[,] BoardNumbers;
    public int[,] BoardNumbersTemp;

    // Simulation control
    public float TimeStep = 1f;

    // Materials
    public Material Alive;
    public Material Dead;

	// Use this for initialization
	void Start () {

        // allocate space for our transforms
        boardTiles = new MeshRenderer[BoardSize,BoardSize];
        BoardNumbers = new int[BoardSize,BoardSize];
        BoardNumbersTemp = new int[BoardSize, BoardSize];

        // spawn the board
	    for (int i = 0; i < BoardSize; i++)
	    {
	        for (int j = 0; j < BoardSize; j++)
	        {
	            Transform currentTile = Instantiate(Tile, new Vector3(i, 0f, j), Quaternion.identity) as Transform;
	            // store tiles in an array
                boardTiles[i, j] = currentTile.GetComponent<MeshRenderer>();
	            //BoardNumbers[i, j] = Random.Range(0, 2);
	        }
	    }

        // start the game routine
	    StartCoroutine("Game");

	}

    public IEnumerator Game()
    {
        while (true)
        {
            int totalPopulation = 0;
            // Simulation
            for (int i = 1; i < BoardSize - 1; i++)
            {
                for (int j = 1; j < BoardSize - 1; j++)
                {
                    //count the total population
                    if (BoardNumbers[i, j] == 1)
                    {
                        totalPopulation++;
                    }

                    //Process neighborhood of a cell
                    int sum = 0;
                    //Get neighborhood
                    sum = BoardNumbers[i + 1, j + 0] + BoardNumbers[i + 1, j + 1] + BoardNumbers[i + 0, j + 1] + BoardNumbers[i - 1, j + 0] +BoardNumbers[i - 1, j - 1] + BoardNumbers[i + 1, j - 1] + BoardNumbers[i - 1, j + 1] + BoardNumbers[i + 0, j - 1];

                    //Any live cell with fewer than two live neighbours dies, as if caused by under-population.
                    if (BoardNumbers[i, j] == 1 && sum < 2)
                    {
                        BoardNumbersTemp[i, j] = 0;
                    }
                    //Any live cell with two or three live neighbours lives on to the next generation.
                    if (BoardNumbers[i, j] == 1 &&  (sum == 2 || sum == 3))
                    {
                        BoardNumbersTemp[i, j] = 1;
                    }
                    //Any live cell with more than three live neighbours dies, as if by over-population.
                    if (BoardNumbers[i, j] == 1 && sum > 3)
                    {
                        BoardNumbersTemp[i, j] = 0;
                    }
                    //Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
                    if (BoardNumbers[i, j] == 0 && sum == 3)
                    {
                        BoardNumbersTemp[i, j] = 1;
                    }
                }
            }

            // Overwrite old values with new ones
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    BoardNumbers[i, j] = BoardNumbersTemp[i, j];
                }
            }

            //Redraw the board
            for (int i = 1; i < BoardSize - 1; i++)
            {
                for (int j = 1; j < BoardSize - 1 ; j++)
                {
                    if(BoardNumbers[i,j] == 1)
                    {
                        boardTiles[i, j].material = Alive;
                    }
                    else if (BoardNumbers[i, j] == 0)
                    {
                        boardTiles[i, j].material = Dead;
                    }
                }
            }
            // Simulation end
            yield return new WaitForSeconds(TimeStep);
        }
        yield break;
    }
	
}
