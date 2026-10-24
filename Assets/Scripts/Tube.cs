using UnityEngine;
using System.Collections.Generic;
using System;

public class Tube : MonoBehaviour
{
    public Stack<int> colors = new Stack<int>();
    private int MaxCapacity = 4;
    public GameObject unitPrefab;
    private List<GameObject> units = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        GameManager.Instance.OnTubeClicked(this);
    }
    
    public int Peek()
    {
        // return the color on top, without removing it
        return colors.Peek();
    }

    public bool IsEmpty()
    {

        if(colors.Count == 0) return true;
        else return false;
    }
    public bool IsFull()
    {
        // true if colors.Count has reached capacity
        if(colors.Count == MaxCapacity) return true;
        else return false;
    }

    public bool IsSolved()
    {
        // true if the tube is full AND every color in it is the same
        if (IsFull())
        {
            foreach (int i in colors)
            {
                if (i != Peek())
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    public bool CanPour(Tube target)
    {
        // use IsEmpty / IsFull / Peek to check the three conditions above
        if (!IsEmpty() && !target.IsFull() && (target.IsEmpty() || target.Peek() == Peek()))
        {
            return true;
        }
        return false;
    }

    public void PourInto(Tube target)
    {
        // 1. figure out how many matching-color units are on top of this tube
        int matchingColor = 0;
        foreach (int color in colors)
        {
            if(Peek() == color) matchingColor++;
            else break;
        }
        // 2. figure out how much room is left in target
        int leftSpaceTarget = target.MaxCapacity - target.colors.Count;

        // 3. move the smaller of those two counts, one unit at a time, from this tube to target
        

        for (int i = 0; i < Math.Min(matchingColor, leftSpaceTarget); i++ )
        {
            int moved = colors.Pop();
            target.colors.Push(moved);
        }
    }

    public void Redraw()
    {
        foreach(GameObject unit in units)
        {
            Destroy(unit);
        }
        units.Clear();

        int[] arr = colors.ToArray();
        System.Array.Reverse(arr);

        for(int i = 0; i<arr.Length; i++)
        {
            Vector3 position = new (transform.position.x, transform.position.y - 2f + 0.5f + i);
            GameObject square = Instantiate(unitPrefab, position, Quaternion.identity);
            square.GetComponent<SpriteRenderer>().color = GameManager.Instance.palette[arr[i]];
            units.Add(square);
        }
    }

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else GetComponent<SpriteRenderer>().color = Color.white;
    }
}
