using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour
{
    private bool isRunning = true;
    public Camera camera;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update () 
    {
        //pause/play controls
	    if (Input.GetKeyUp(KeyCode.P) && isRunning)
	    {
	        Board.Instance.StopCoroutine("Game");
	        isRunning = false;
	    }      
	    else if (Input.GetKeyUp(KeyCode.P) && !isRunning)
	    {
	        Board.Instance.StartCoroutine("Game");
	        isRunning = true;
	    }
        //mouse controls
	    if (Input.GetMouseButtonDown(0))
	    {
	        RaycastHit hit;
	        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

	        if (Physics.Raycast(ray, out hit))
	        {
	            Vector3 hitAdjusted = hit.point + new Vector3(0.5f, 0, 0.5f);
	            int x = (int)hitAdjusted.x;
	            int z = (int)hitAdjusted.z;

	            if (Board.Instance.BoardNumbers[x, z] == 0)
	            {
	                Board.Instance.BoardNumbers[x, z] = 1;
	                Board.Instance.boardTiles[x, z].material = Board.Instance.Alive;
	            }
                else if (Board.Instance.BoardNumbers[x, z] == 1)
                {
                    Board.Instance.BoardNumbers[x, z] = 0;
                    Board.Instance.boardTiles[x, z].material = Board.Instance.Dead;
                }
	        }
	    }

	
	}
}
