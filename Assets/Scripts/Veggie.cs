using UnityEngine;

public class Veggie : MonoBehaviour
{
    [SerializeField] private string veggieType;
    public Vector2 gridPosition;

    private void Start()
    {
        float xPos = gameObject.transform.position.x;
        float yPos = gameObject.transform.position.y;
        gridPosition = new Vector2(xPos, yPos);

        //add to grid array
        GridInfo gridInfo = GameObject.Find("DragDrop").GetComponent<GridInfo>();
        int xArrayPos = (int) xPos + 2;
        int yArrayPos = (int) (yPos + 2.5);
        gridInfo.grid[xArrayPos, yArrayPos] = gameObject;
    }
    public void UpdateGridPosition(Vector2 newPosition)
    {
        gridPosition = newPosition;
    }
    public void CheckIf3Matched()
    {
        Debug.Log("Check if 3 tiles have been matched");
    }
}
