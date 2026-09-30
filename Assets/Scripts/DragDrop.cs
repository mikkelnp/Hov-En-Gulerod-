using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragDrop : MonoBehaviour
{
    private Camera gameCamera;
    private Collider2D movedVeggie;
    private Vector2 gridPosition;
    float xPos;
    float yPos;
    //[SerializeField] private Vector2 gridOffset;
    private Vector3 newPosition;
    private GridInfo gridInfo;

    void Start()
    {
        gameCamera = Camera.main;
        gridInfo = GameObject.Find("DragDrop").GetComponent<GridInfo>();
    }

    void Update()
    {
        // Henter det lag, hvor man kan trække veggies i:
        int veggieLayer = LayerMask.GetMask("Veggies");

        if (Touchscreen.current == null) return;

        var touch = Touchscreen.current.primaryTouch;

        // Omsætning af skærmposition til verdensposition:
        Vector2 screenPosition = touch.position.ReadValue();
        Vector3 worldPosition =
            gameCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 0));

        worldPosition.z = 0;

        // When first pressed
        if (touch.press.wasPressedThisFrame)
        {
            movedVeggie = Physics2D.OverlapPoint(worldPosition, veggieLayer);
            if (movedVeggie != null)
            {
                gridPosition = movedVeggie.GetComponent<Veggie>().gridPosition;
            }
        }

        // While held down
        if (movedVeggie != null && touch.press.isPressed)
        {
            // xPosition

            if (worldPosition.y >= gridPosition.y + 0.5 || worldPosition.y <= gridPosition.y - 0.5)
            {
                if (worldPosition.x <= gridPosition.x - 0.5)
                {
                    xPos = gridPosition.x - 0.5f;
                }
                else if (worldPosition.x >= gridPosition.x + 0.5)
                {
                    xPos = gridPosition.x + 0.5f;
                }
                else
                {
                    xPos = worldPosition.x;
                }
            }
            else
            {
                if (worldPosition.x <= gridPosition.x - 1)
                {
                    xPos = gridPosition.x - 1;
                }
                else if (worldPosition.x >= gridPosition.x + 1)
                {
                    xPos = gridPosition.x + 1;
                }
                else
                {
                    xPos = worldPosition.x;
                }
            }

            // yPosition
            if (worldPosition.x >= gridPosition.x + 0.5 || worldPosition.x <= gridPosition.x - 0.5)
            {
                if (worldPosition.y <= gridPosition.y - 0.5)
                {
                    yPos = gridPosition.y - 0.5f;
                }
                else if (worldPosition.y >= gridPosition.y + 0.5)
                {
                    yPos = gridPosition.y + 0.5f;
                }
                else
                {
                    yPos = worldPosition.y;
                }
            }
            else {
                if (worldPosition.y <= gridPosition.y - 1)
                {
                    yPos = gridPosition.y - 1;
                }
                else if (worldPosition.y >= gridPosition.y + 1)
                {
                    yPos = gridPosition.y + 1;
                }
                else
                {
                    yPos = worldPosition.y;
                }
            }

            newPosition = new Vector3(xPos, yPos, 0);
            movedVeggie.transform.position = newPosition;
        }

        // When released
        if (touch.press.wasReleasedThisFrame)
        {
            if (movedVeggie != null)
            {   //Snap to nearest tile

                //Check new xPosition compared to old
                if (newPosition.x > gridPosition.x + 0.5)       { xPos =  1; }
                else if (newPosition.x < gridPosition.x - 0.5)  { xPos = -1; }
                else                                            { xPos =  0; }

                //Check new yPosition compared to old
                if (newPosition.y > gridPosition.y + 0.5)       { yPos =  1; }
                else if (newPosition.y < gridPosition.y - 0.5)  { yPos = -1; }
                else                                            { yPos =  0; }

                //snap to correct tile
                movedVeggie.transform.position = new Vector3(gridPosition.x + xPos, gridPosition.y + yPos, 0);

                Vector2 newGridPosition;
                newGridPosition.x = gridPosition.x + xPos;
                newGridPosition.y = gridPosition.y + yPos;

                Veggie veggie = movedVeggie.GetComponent<Veggie>();

                //update gridPosition
                veggie.UpdateGridPosition(newGridPosition);

                //move other tile to previous square
                SwitchTiles(newGridPosition);


                //Check if 3 tiles have been matched
                veggie.CheckIf3Matched();
            }
        }
    }
    void SwitchTiles(Vector2 newGridPosition)
    {
        //get array coordinates
        int xArrayPosOld = (int)gridPosition.x + 2;
        int yArrayPosOld = (int)(gridPosition.y + 2.5);
        int xArrayPosNew = (int)newGridPosition.x + 2;
        int yArrayPosNew = (int)(newGridPosition.y + 2.5);

        //switch positions in array
        GameObject veggie2 = gridInfo.grid[xArrayPosNew, yArrayPosNew];
        gridInfo.grid[xArrayPosNew, yArrayPosNew] = movedVeggie.gameObject;
        gridInfo.grid[xArrayPosOld, yArrayPosOld] = veggie2;

        Debug.Log("arrayPosNew name = " + gridInfo.grid[xArrayPosNew, yArrayPosNew].name);
        Debug.Log("arrayPosOld name = " + gridInfo.grid[xArrayPosOld, yArrayPosOld].name);

        //Switch positions on board
        Vector3 newPositionVeggie2 = veggie2.GetComponent<Transform>().transform.position;
        if (gridPosition.x > newGridPosition.x)
        {
            newPositionVeggie2.x += 1;
        }
        else if (gridPosition.x < newGridPosition.x)
        {
            newPositionVeggie2.x -= 1;
        }
        else if (gridPosition.y > newGridPosition.y)
        {
            newPositionVeggie2.y += 1;
        }
        else if (gridPosition.y < newGridPosition.y)
        {
            newPositionVeggie2.y -= 1;
        }
        veggie2.GetComponent<Transform>().transform.position = newPositionVeggie2;

        //update veggie2 grid position
        veggie2.GetComponent<Veggie>().UpdateGridPosition(gridPosition);
    }
}
