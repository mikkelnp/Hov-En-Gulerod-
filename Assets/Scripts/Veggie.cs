using UnityEngine;

public class Veggie : MonoBehaviour
{
    [SerializeField] private string veggieType;
    public Vector2 gridPosition;

    private void Start()
    {
        float xPos = gameObject.transform.position.x + 2.5f;
        float yPos = gameObject.transform.position.y + 3.5f;
        gridPosition = new Vector2(xPos, yPos);
        Debug.Log(gridPosition);
    }

    public void CheckIf3Matched()
    {

    }
}
