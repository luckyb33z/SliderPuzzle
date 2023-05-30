using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMovementTest : MonoBehaviour
{

    [SerializeField] GameObject tileToMove;
    [SerializeField] float movementDistance = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DoTileMovement();
    }

    private void DoTileMovement()
    {
        
        Vector2 movementVector = GetMovementVector() * movementDistance;

        if (tileToMove != null)
        {
            tileToMove.transform.Translate(movementVector);
        }

    }

    private Vector2 GetMovementVector()
    {
        int xMovement = 0;
        int yMovement = 0;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            xMovement = -1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            xMovement = 1;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            yMovement = 1;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            yMovement = -1;
        }

        return new Vector2(xMovement, yMovement);
    }

}
