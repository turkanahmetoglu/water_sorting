using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Tube selectedTube;

    void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            }
            selectedTube = null;
            Debug.Log(clicked.colors.Count);
        }
    }
}
