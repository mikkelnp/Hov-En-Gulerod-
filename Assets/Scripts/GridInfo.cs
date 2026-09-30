using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GridInfo : MonoBehaviour
{
    public GameObject[,] grid = new GameObject[5,8];
    [SerializeField] private GameObject apple;

    public void MoveDown(List<int> xWhereErased)
    {
        foreach (int x in xWhereErased)
        {
            for (int y = 1; y < 8; y++)
            {
                if (grid[x, y - 1] == null)
                {
                    if (grid[x, y] != null)
                    {
                        grid[x, y].GetComponent<Veggie>().MoveDown();
                    }
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;

                    if (y == 7)
                    {
                        GameObject newVeggie = Instantiate(apple, new Vector3(x - 2, 4.5f, 0), new Quaternion());
                        grid[x, y] = newVeggie;
                    }

                }
            }
        }
    }
    public void AddToArray(GameObject item, int x, int y)
    {
        grid[x, y] = item;
    }
    public void RemoveFromArray(int x, int y)
    {
        grid[x, y] = null;
    }
    private void AddRandomToTopRow()
    {

    }
}
