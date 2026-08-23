using UnityEngine;
using UnityEngine.UI;

public class FadeEvent : MonoBehaviour
{

    private void Start()
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            Color color = image.color;
            color.a = 1f;
            image.color = color;
            
            gameObject.SetActive(true);
        }
    }

    public void CallDisablePanel()
    {
        SceneTransitionManager manager = GetComponentInParent<SceneTransitionManager>();

        if (manager != null)
        {
            manager.DisableFadePanel();
        }
        else
        {
            Debug.LogError("FadeEventBridge cannot find SceneTransitionManager on parent!");
        }
    }
}