using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private Stack<int[][]> history = new Stack<int[][]>();
    public static GameManager Instance;
    private Tube selectedTube;
    public List<Tube> tubes;
    public Color[] palette;
    private int currentLevel = 0;
    private bool isAnimating = false;

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
        //StartCoroutine(MoveUnit(tubes[0].GetTopUnits(1)[0], tubes[3].SlotPosition(0), 0.3f));
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
        if (isAnimating)
        {
            return;
        }
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
                StartCoroutine(PourRoutine(selectedTube, clicked));
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
        if (isAnimating)
        {
            return;
        }

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
        if (isAnimating)
        {
            return;
        }

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

    IEnumerator MoveUnit(GameObject unit, Vector3 destination, float duration)
    {
        Vector3 start = unit.transform.position;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            unit.transform.position = Vector3.Lerp(start, destination, t / duration);
            yield return null;
        }

        unit.transform.position = destination;
    }

    IEnumerator PourRoutine(Tube from, Tube to)
    {
        isAnimating = true;
        float duration = 0.3f;

        int count = from.CountToPour(to);
        List<GameObject> units = from.GetTopUnits(count);
        int startSlot = to.colors.Count;

        for (int k = 0; k < count; k++)
        {
            StartCoroutine(MoveUnit(units[count - 1 - k], to.SlotPosition(startSlot + k), duration));
        }

        yield return new WaitForSeconds(duration);

        from.PourInto(to);
        from.Redraw();
        to.Redraw();

        isAnimating = false;

        if (CheckWin())
        {
            Debug.Log("You win!");
            NextLevel();
        }
    }
}
