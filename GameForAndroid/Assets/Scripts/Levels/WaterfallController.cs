using UnityEngine;

public class WaterfallController : MonoBehaviour
{
    [Header("Particle")]
    public GameObject waterfallVisual;  

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Trigger hit by: " + collision.name);

        if (collision.CompareTag("Plug"))
            waterfallVisual.SetActive(false);
    }

}
