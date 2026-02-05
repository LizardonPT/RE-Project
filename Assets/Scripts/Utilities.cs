using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Utilities
{
    public static Vector2 Convert3Dto2D(
        Vector3 ponto,
        Vector3 origem,
        Vector3 eixoX,
        Vector3 eixoY,
        Vector3 normal,
        out bool inFront
    )
    {
        Vector3 deslocamento = ponto - origem;

        float x = Vector3.Dot(deslocamento, eixoX);
        float y = Vector3.Dot(deslocamento, eixoY);

        inFront = Vector3.Dot(deslocamento, normal) > 0f;

        return new Vector2(x, y);
    }

    public static Vector2 GetKeyboardInputs()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
            return Vector2.zero;

        int w = keyboard.wKey.isPressed ? 1 : 0;
        int s = keyboard.sKey.isPressed ? -1 : 0;
        int a = keyboard.aKey.isPressed ? -1 : 0;
        int d = keyboard.dKey.isPressed ? 1 : 0;

        return new Vector2(w + s, a + d);
    }

    public static Vector2 GetMouseInputs()
    {
        Mouse mouse = Mouse.current;
        Vector2 delta = mouse.delta.ReadValue();
        return new Vector2(delta.x, delta.y);
    }
}
