using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Veggie : MonoBehaviour
{
    public string veggieType;
    public Vector2 gridPosition;
    private GridInfo gridInfo;
    private DragDrop dragDrop;
    private int xArrayPos;
    private int yArrayPos;
    private void Start()
    {
        float xPos = gameObject.transform.position.x;
        float yPos = gameObject.transform.position.y;
        gridPosition = new Vector2(xPos, yPos);

        dragDrop = GameObject.Find("DragDrop").GetComponent<DragDrop>();

        //add to array
        gridInfo = GameObject.Find("DragDrop").GetComponent<GridInfo>();
        xArrayPos = (int) xPos + 2;
        yArrayPos = (int) (yPos + 2.5);
        gridInfo.AddToArray(gameObject, xArrayPos, yArrayPos);
    }
    public void UpdateGridPosition(Vector2 newPosition, int x, int y)
    {
        gridPosition = newPosition;
        xArrayPos = x;
        yArrayPos = y;
    }
    public void MoveDown()
    {
        Vector3 pos = GetComponent<Transform>().position;
        pos.y -= 1;
        GetComponent<Transform>().position = pos;
        yArrayPos -= 1;
        gridPosition.y -= 1;
    }
    public List<int> CheckIf3Matched()
    {
        bool nextVeggieIsSame = true;
        int adjacentVeggie = 1;
        List<Vector2> solvedHorizontal = new List<Vector2>();
        List<Vector2> solvedVertical = new List<Vector2>();
        solvedHorizontal.Add(new Vector2 (xArrayPos, yArrayPos));
        solvedVertical.Add(new Vector2(xArrayPos, yArrayPos));
        List<int> xWhereErasedFrom = new List<int>();
        bool horizontalSolved = false;
        bool verticalSolved = false;


        //check to the right
        do
        {
            if (xArrayPos + adjacentVeggie < 5 &&
                gridInfo.grid[xArrayPos + adjacentVeggie, yArrayPos] != null &&
                veggieType == gridInfo.grid[xArrayPos + adjacentVeggie, yArrayPos].GetComponent<Veggie>().veggieType)
            {
                solvedHorizontal.Add(new Vector2 (xArrayPos + adjacentVeggie, yArrayPos));
                adjacentVeggie++;
            }
            else
            {
                nextVeggieIsSame = false;
                adjacentVeggie = 1;
            }
        }
        while (nextVeggieIsSame);
        nextVeggieIsSame = true;

        //check to the left
        do
        {
            if (xArrayPos - adjacentVeggie > -1 &&
                gridInfo.grid[xArrayPos - adjacentVeggie, yArrayPos] != null &&
                veggieType == gridInfo.grid[xArrayPos - adjacentVeggie, yArrayPos].GetComponent<Veggie>().veggieType)
            {
                solvedHorizontal.Add(new Vector2(xArrayPos - adjacentVeggie, yArrayPos));
                adjacentVeggie++;
            }
            else
            {
                nextVeggieIsSame = false;
                adjacentVeggie = 1;
            }
        }
        while (nextVeggieIsSame);
        nextVeggieIsSame = true;

        //check above
        do
        {
            if (yArrayPos + adjacentVeggie < 8 &&
                gridInfo.grid[xArrayPos, yArrayPos + adjacentVeggie] != null &&
                veggieType == gridInfo.grid[xArrayPos, yArrayPos + adjacentVeggie].GetComponent<Veggie>().veggieType)
            {
                solvedVertical.Add(new Vector2(xArrayPos, yArrayPos + adjacentVeggie));
                adjacentVeggie++;
            }
            else
            {
                nextVeggieIsSame = false;
                adjacentVeggie = 1;
            }
        }
        while (nextVeggieIsSame);
        nextVeggieIsSame = true;

        //check below
        do
        {
            if (yArrayPos - adjacentVeggie > -1 &&
                gridInfo.grid[xArrayPos, yArrayPos - adjacentVeggie] != null &&
                veggieType == gridInfo.grid[xArrayPos, yArrayPos - adjacentVeggie].GetComponent<Veggie>().veggieType)
            {
                solvedVertical.Add(new Vector2(xArrayPos, yArrayPos - adjacentVeggie));

                adjacentVeggie++;
            }
            else
            {
                nextVeggieIsSame = false;
                adjacentVeggie = 1;
            }
        }
        while (nextVeggieIsSame);

        //if 3 or more in a row
        if (solvedHorizontal.Count > 2 || solvedVertical.Count > 2) {
            if (solvedHorizontal.Count > 2)
            {
                horizontalSolved = true;
                Debug.Log(solvedHorizontal.Count + " in a row!");

                if (veggieType == dragDrop.targetVeggie)
                {
                    dragDrop.amountMatched += solvedHorizontal.Count - 2;
                }

                foreach (Vector2 i in solvedHorizontal)
                {
                    int x = (int)i.x;
                    int y = (int)i.y;
                    xWhereErasedFrom.Add(x);
                    GameObject obj = gridInfo.grid[x, y];
                    Destroy(obj);
                    gridInfo.RemoveFromArray(x, y);
                }
            }
            if (solvedVertical.Count > 2)
            {
                verticalSolved = true;
                Debug.Log(solvedVertical.Count + " in a column!");

                if (veggieType == dragDrop.targetVeggie)
                {
                    dragDrop.amountMatched += solvedVertical.Count - 2;
                }

                foreach (Vector2 i in solvedVertical)
                {
                    int x = (int)i.x;
                    int y = (int)i.y;
                    xWhereErasedFrom.Add(x);
                    GameObject obj = gridInfo.grid[x, y];
                    Destroy(obj);
                    gridInfo.RemoveFromArray(x, y);
                }
            }


            //remove duplicate if horizontal and vertical solved at same time
            if (horizontalSolved && verticalSolved)
            {
                xWhereErasedFrom.RemoveAt(0);
            }


            foreach (int x in xWhereErasedFrom)
            {
                Debug.Log("xposition " + x);
            }



            //move tiles down
            return xWhereErasedFrom;
        }
        return null;
    }
}
