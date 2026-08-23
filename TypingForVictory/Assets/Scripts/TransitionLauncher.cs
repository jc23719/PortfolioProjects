using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionLauncher : MonoBehaviour {
    public GameObject transitionManagerPrefab;
    
    void Awake() {
        // Check if a SceneTransitionManager instance already exists in the game
        if (SceneTransitionManager.Instance == null) {
            if (transitionManagerPrefab != null)  {
                GameObject manager = Instantiate(transitionManagerPrefab);
            }
            else
            {
                Debug.LogError("Transition Manager Prefab is not assigned in the Launcher!");
            }
        }
    }
}