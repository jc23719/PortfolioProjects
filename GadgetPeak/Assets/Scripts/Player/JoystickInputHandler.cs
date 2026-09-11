using UnityEngine;

public class JoystickInputHandler : MonoBehaviour {
    public JoystickMovement joystick;
    public PlayerController player;
    public float jumpThreshold = 0.5f;

    private bool jumpTriggered = false;

    void Update() {
        // Move left or right
        float moveX = joystick.Horizontal();
        player.Move(moveX);

        // Triggers a jump once per upward pull from player
        Vector2 dir = joystick.Direction(); 
        float magnitude = dir.magnitude;
        float angle = Vector2.Angle(dir, Vector2.up); 
        
        if (magnitude > 0.5f && angle < 60f)
        {
            if (!jumpTriggered)
            {
                player.Jump();
                jumpTriggered = true;
                Debug.Log("Jump called from joystick");
            }
        }
        else
        {
            jumpTriggered = false;
        }
        
    }
}
