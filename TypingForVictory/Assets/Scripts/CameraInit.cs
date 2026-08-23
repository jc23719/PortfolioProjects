using UnityEngine;

public class CameraInit : MonoBehaviour
{
    void Start()
    {
        if (SceneTransitionManager.Instance != null)
        {
            Canvas canvas = SceneTransitionManager.Instance.GetComponent<Canvas>();
            
            // Check if the Canvas needs its render camera set
            if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                canvas.worldCamera = GetComponent<Camera>();
            }
        }
    }
}