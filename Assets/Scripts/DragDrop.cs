using UnityEngine;
using UnityEngine.InputSystem;

public class DragDrop : MonoBehaviour
{
    private Camera gameCamera;
    private Collider2D movedVeggie;
    private Vector2 gridPosition;
    float xPos;
    float yPos;
    [SerializeField] private Vector2 gridOffset;
    private Vector3 newPosition;
    void Start()
    {
        gameCamera = Camera.main;
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

        // Finger ned
        if (touch.press.wasPressedThisFrame)
        {
            movedVeggie = Physics2D.OverlapPoint(worldPosition, veggieLayer);
            if (movedVeggie != null)
            {
                gridPosition = movedVeggie.GetComponent<Veggie>().gridPosition;
            }
        }

        // Finger holdes nede
        if (movedVeggie != null && touch.press.isPressed)
        {
            // xPosition
            if (worldPosition.x <= gridPosition.x - 1 + gridOffset.x)
            {
                xPos = gridPosition.x - 1 + gridOffset.x;
            }
            else if (worldPosition.x >= gridPosition.x + 1 + gridOffset.x)
            {
                xPos = gridPosition.x + 1 + gridOffset.x; ;
            }
            else
            {
                xPos = worldPosition.x;
            }
            
            // yPosition
            if (worldPosition.y <= gridPosition.y - 1 + gridOffset.y)
            {
                yPos = gridPosition.y - 1 + gridOffset.y;
            }
            else if (worldPosition.y >= gridPosition.y + 1 + gridOffset.y)
            {
                yPos = gridPosition.y + 1 + gridOffset.y;
            }
            else
            {
                yPos = worldPosition.y;
            }

            Debug.Log("xPos = " + xPos + " yPos = " + yPos);

            newPosition = new Vector3(xPos, yPos, 0);

            movedVeggie.transform.position = newPosition;
        }

        // Finger slippes
        if (touch.press.wasReleasedThisFrame)
        {
            if (movedVeggie != null)
            {
                //Snap to nearest tile
                //Check new xPosition compared to old
                if (newPosition.x > gridPosition.x + 0.5 + gridOffset.x)        { xPos =  1; }
                else if (newPosition.x < gridPosition.x - 0.5 + gridOffset.x)   { xPos = -1; }
                else                                                            { xPos =  0; }

                //Check new yPosition compared to old
                if (newPosition.y > gridPosition.y + 0.5 + gridOffset.y)        { yPos =  1; }
                else if (newPosition.y < gridPosition.y - 0.5 + gridOffset.y)   { yPos = -1; }
                else                                                            { yPos =  0; }


                //snap to correct tile
                movedVeggie.transform.position = new Vector3(gridPosition.x + xPos + gridOffset.x, gridPosition.y + yPos + gridOffset.y, 0);


                //Veggie veggie = movedVeggie.GetComponent<Veggie>();
                //veggie.TjekOmKorrektPlads();
            }
        }
    }
}
