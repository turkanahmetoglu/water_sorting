using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Tube selectedTube;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
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
    }
}
