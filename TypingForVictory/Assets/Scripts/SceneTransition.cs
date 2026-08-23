using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour{
    private Animator _fadeAnimator;

    public static SceneTransitionManager Instance;

    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 

            _fadeAnimator = GetComponentInChildren<Animator>(); 
            
            if (_fadeAnimator == null)
            {
                Debug.LogError("FATAL ERROR: SceneTransitionManager could not find the Animator component on its children.");
            }
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            // Destroy duplicate instances
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // handles the Fade In logic runs after a scene load completes
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if (mode == LoadSceneMode.Single) {
            if (!gameObject.activeSelf) gameObject.SetActive(true);

            if (_fadeAnimator != null) {
                StartCoroutine(ManualFadeIn(1.0f));
            }
        }
    }
    
    private IEnumerator ManualFadeIn(float duration) {
        // Ensure the FadePanel is active before starting the fade
        if (_fadeAnimator.gameObject.activeSelf == false) {
            _fadeAnimator.gameObject.SetActive(true);
        }
        
        Image fadeImage = _fadeAnimator.gameObject.GetComponent<Image>();
        if (fadeImage == null) yield break;

        float startAlpha = 1f; // Start opaque black
        float endAlpha = 0f;   // End transparent
        float time = 0f;

        Color color = fadeImage.color;
        color.a = startAlpha;
        fadeImage.color = color;

        // Loop to animate the fade
        while (time < duration) {
            time += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            
            color.a = currentAlpha;
            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;

        DisableFadePanel(); 
    }

    public void LoadScene(string sceneName) {
        StartCoroutine(Transition(sceneName));
    }

   private IEnumerator Transition(string sceneName) {
        if (_fadeAnimator != null)
        {
            _fadeAnimator.gameObject.SetActive(true);
            
            _fadeAnimator.Play("FadeOut", 0);

            // Wait for animation to finish
            yield return new WaitForSeconds(1.0f); 
        }

        SceneManager.LoadScene(sceneName);
    }

    public void DisableFadePanel() {
        if (_fadeAnimator != null && _fadeAnimator.gameObject.activeSelf)  {
            _fadeAnimator.gameObject.SetActive(false); 
        }
    }
}