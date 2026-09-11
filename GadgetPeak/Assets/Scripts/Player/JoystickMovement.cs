using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickMovement : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform joystickBG;
    public RectTransform joystickKnob;
    public float maxRadius = 100f;
    private Vector2 smoothInput;
    public float smoothSpeed = 10f;


    private Vector2 inputVector;

    public void OnPointerDown(PointerEventData eventData) {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBG, eventData.position, eventData.pressEventCamera, out pos);

        pos = Vector2.ClampMagnitude(pos, maxRadius);
        joystickKnob.anchoredPosition = pos;
        inputVector = pos / maxRadius;

        Debug.Log("Joystick Vertical: " + inputVector.y);
    }

    public void OnPointerUp(PointerEventData eventData) {
        inputVector = Vector2.zero;
        joystickKnob.anchoredPosition = Vector2.zero;
    }

    // public float Horizontal() => inputVector.x;
    // public float Vertical() => inputVector.y;
    // public Vector2 Direction() => inputVector;
    public float Horizontal()
    {
        smoothInput = Vector2.Lerp(smoothInput, inputVector, Time.deltaTime * smoothSpeed);
        return smoothInput.x;
    }

    public float Vertical() => inputVector.y;
    public Vector2 Direction()
    {
        smoothInput = Vector2.Lerp(smoothInput, inputVector, Time.deltaTime * smoothSpeed);
        return smoothInput;
    }
}
