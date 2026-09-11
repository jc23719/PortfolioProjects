using UnityEngine;

public class GadgetGhost : MonoBehaviour
{
    public void SetValid(bool valid)
    {
        // Green if valid, red if invalid
        Color c = valid ? new Color(0, 1, 0, 0.4f) : new Color(1, 0, 0, 0.4f);
        GetComponent<SpriteRenderer>().color = c;
    }
}
