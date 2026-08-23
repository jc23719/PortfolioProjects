using UnityEngine;
using TMPro;

public class LetterDisplay : MonoBehaviour {
    public TextMeshPro letterText;
    private Camera mainCamera;
    private string fontResourcePath = "Fonts/Gothic_pixel_font(0S) SDF"; 
    private TMP_FontAsset customFont;
    [SerializeField] private float customFontSize = 10f;

    void Start() {
        customFont = Resources.Load<TMP_FontAsset>(fontResourcePath);
        if (customFont != null && letterText != null) {
            letterText.font = customFont;
            letterText.fontSize = customFontSize;
        } else {
            Debug.LogWarning($"LetterDisplay: Could not load font at {fontResourcePath}");
        }
        if (letterText == null) {
            Debug.LogError("Lettertext==null");
            return;
        } 
        Refresh();
        mainCamera = Camera.main;
    }
    void LateUpdate() {
        if (mainCamera != null) {
            // Make the text face the camera
            transform.forward = mainCamera.transform.forward;
        }
        Refresh();
    }

    public void Refresh() {
        Enemy enemy = GetComponentInParent<Enemy>();
        WordEnemy wordEnemy = GetComponentInParent<WordEnemy>();
        ParticleAttack particleAttack = GetComponentInParent<ParticleAttack>();
        BossEnemy bossEnemy = GetComponentInParent<BossEnemy>();

        if (letterText == null) return;

        if (enemy != null) {
            letterText.text = enemy.assignedLetter.ToString();
        } else if (wordEnemy != null) {
            letterText.text = wordEnemy.assignedWord;
        } else if (particleAttack != null) {
            if (!string.IsNullOrEmpty(particleAttack.assignedWord)) {
                UpdateTextProgress(particleAttack.assignedWord, particleAttack.currentIndex);
            }
        } else if (bossEnemy != null) {
            string currentWord = bossEnemy.GetCurrentWord();
            int progress = bossEnemy.GetCurrentWordProgress();
            
            if (!string.IsNullOrEmpty(currentWord)) {
                UpdateTextProgress(currentWord, progress);
            } else {
                letterText.text = "NEXT WORD"; 
            }
        } else {
            letterText.text = "?"; // fallback
            Debug.LogWarning("LetterDisplay couldn't find Enemy or WordEnemy");
        }
    }

    public void UpdateTextProgress(string word, int progress) {
        if (progress > 0) {
            string untypedPart = word.Substring(progress);
            string typedPart = word.Substring(0, progress);
            
            // Highlight the typed part green and keep the rest white
            letterText.text = $"<color=#00FF00>{typedPart}</color>{untypedPart}";
        } else {
            letterText.text = word;
        }
    }
}
