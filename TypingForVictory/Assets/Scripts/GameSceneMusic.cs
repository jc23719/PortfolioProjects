using UnityEngine;

public class GameSceneMusic : MonoBehaviour {
    void Start() {
        if (AudioManager.Instance != null) {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.gameMusic);
        }
    }
}
