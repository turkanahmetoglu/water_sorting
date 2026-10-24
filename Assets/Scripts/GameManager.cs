using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    private Stack<int[][]> history = new Stack<int[][]>();
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Undo();
        }
    }

    public void OnTubeClicked(Tube clicked)
    {
        if (selectedTube == null)
        {
            selectedTube = clicked;
            clicked.SetSelected(true);
            
            
            Debug.Log("select");
        }
        else if (clicked == selectedTube){
            selectedTube.SetSelected(false);
            selectedTube = null;
            Debug.Log("deselect");
        }
        else
        {
            if (selectedTube.CanPour(clicked))
            {
                history.Push(TakeSnapshot());
                selectedTube.PourInto(clicked);
                selectedTube.Redraw();
                clicked.Redraw();
                if (CheckWin())
                {
                    Debug.Log("You win!");
                    NextLevel();
                }
            }
            //Debug.Log("can't pour");
            selectedTube.SetSelected(false);
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
        history.Clear();
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

    public void RestartLevel()
    {
        // reload the level you're on
        LoadLevel(currentLevel);

        // clear any tube the player had selected
        if (selectedTube)
        {
            selectedTube.SetSelected(false);
            selectedTube = null;
        }
    }
    int[][] TakeSnapshot()
    {
        int[][] snapshot = new int[tubes.Count][];
        for (int i = 0; i < tubes.Count; i++)
        {
            int[] arr = tubes[i].colors.ToArray();
            System.Array.Reverse(arr);
            snapshot[i] = arr;
        }
        return snapshot;
    }

    public void Undo()
    {
        if (selectedTube)
        {
            selectedTube.SetSelected(false);
            selectedTube = null;
        }
        
        if (history.Count > 0)
        {
            int[][] recentSnap = history.Pop();
            for (int i=0; i<tubes.Count; i++)
            {
                tubes[i].colors.Clear();

                foreach (int color in recentSnap[i])
                {
                    tubes[i].colors.Push(color);
                }

                tubes[i].Redraw();
            }

        }
    }
}
