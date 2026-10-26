using UnityEngine;

public class CameraFit : MonoBehaviour
{
    public float worldWidth = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Camera.main.orthographicSize = worldWidth / (2 * Camera.main.aspect);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
