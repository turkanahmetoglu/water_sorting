using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Tube selectedTube;
    public List<Tube> tubes;
    public Color[] palette;
    private int currentLevel = 0;

    private int[][][] levels = new int[][][]
    {
        // Level 1: 3 colors, 5 tubes
        new int[][]
        {
            new int[] { 0, 1, 2, 0 },
            new int[] { 1, 2, 0, 1 },
            new int[] { 2, 0, 1, 2 },
            new int[] { },
            new int[] { }
        },

        // Level 2: 3 colors, 5 tubes
        new int[][]
        {
            new int[] { 1, 0, 2, 1 },
            new int[] { 1, 2, 0, 2 },
            new int[] { 2, 0, 1, 0 },
            new int[] { },
            new int[] { }
        },

        // Level 3: 3 colors, 5 tubes
        new int[][]
        {
            new int[] { 2, 0, 1, 1 },
            new int[] { 1, 2, 0, 2 },
            new int[] { 0, 0, 1, 2 },
            new int[] { },
            new int[] { }
        }

    };

    void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadLevel(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTubeClicked(Tube clicked)
    {
        if (selectedTube == null)
        {
            selectedTube = clicked;
            Debug.Log("select");
        }
        else if (clicked == selectedTube){
            selectedTube = null;
            Debug.Log("deselect");
        }
        else
        {
            if (selectedTube.CanPour(clicked))
            {
                selectedTube.PourInto(clicked);
                selectedTube.Redraw();
                clicked.Redraw();
                if (CheckWin())
                {
                    Debug.Log("You win!");
                    NextLevel();
                }
                
            }
            selectedTube = null;
            //Debug.Log(clicked.colors.Count);
        }
    }

    public bool CheckWin()
    {
        foreach (Tube tube in tubes)
        {
            if (!tube.IsEmpty() && !tube.IsSolved())
            {
                return false;
            }
        }
        return true;
    }

    void LoadLevel(int index)
    {
        currentLevel = index;
        for (int i=0; i<tubes.Count; i++)
        {
            tubes[i].colors.Clear();

            foreach (int color in levels[index][i])
            {
                tubes[i].colors.Push(color);
            }

            tubes[i].Redraw();
        }

    }

    void NextLevel()
    {
        // move to the next level number
        currentLevel += 1;
        // if there is one, load it
        if (levels.Length > currentLevel)
        {
            LoadLevel(currentLevel);
        }
        // if you ran out of levels, print "All levels complete!" instead
        else
        {
            Debug.Log("All levels complete!");
        }
    }
}
