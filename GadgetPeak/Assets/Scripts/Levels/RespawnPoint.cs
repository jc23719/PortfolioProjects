using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public static Transform current;

    private void Awake()
    {
        current = transform;
    }
}
