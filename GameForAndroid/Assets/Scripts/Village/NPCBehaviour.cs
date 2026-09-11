using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCBehaviour : MonoBehaviour
{
    public MiniGameUI miniGameUI;
    private bool hasBeenUsed = false;

    void OnMouseDown()
    {
        if (hasBeenUsed)
            return;

        hasBeenUsed = true;
        string scene = SceneManager.GetActiveScene().name;

        if (scene == "VILLAGE")
            miniGameUI.miniGameType = MiniGameType.Rats;
        else if (scene == "VILLAGE2")
            miniGameUI.miniGameType = MiniGameType.Apples;

        miniGameUI.OpenMiniGame();
    }
}
