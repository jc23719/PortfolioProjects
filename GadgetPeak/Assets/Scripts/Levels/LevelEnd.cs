using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public string nextLevelName;
    public float walkOffSpeed = 3f; 
    public float walkDuration = 1.2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponentInParent<PlayerController>();
            Debug.Log("Player found: " + player);
            StartCoroutine(HandleLevelEnd(player));
        }
    }
    
    private System.Collections.IEnumerator HandleLevelEnd(PlayerController player) {
        // Player walks off screen 
        yield return StartCoroutine(player.WalkOffScreen(walkOffSpeed, walkDuration)); 
        // Fade to next scene 
        Debug.Log("FadeManager.Instance = " + FadeManager.Instance);
        FadeManager.Instance.FadeToScene(nextLevelName);
    }
}
