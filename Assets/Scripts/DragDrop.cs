using UnityEngine;
using UnityEngine.InputSystem;

public class DragDrop : MonoBehaviour
{
    private Camera gameCamera;
    private Collider2D detRamte;

    void Start()
    {
        gameCamera = Camera.main;
    }

    void Update()
    {
        // Henter det lag, hvor man kan trække bogstaver i:
        int traekbartLag = LayerMask.GetMask("Veggies");

        if (Touchscreen.current == null) return;

        var beroering = Touchscreen.current.primaryTouch;

        // Omsætning af skærmposition til verdensposition:
        Vector2 screenPosition = beroering.position.ReadValue();
        Vector3 worldPosition =
            gameCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 0));

        worldPosition.z = 0;

        // Finger ned
        if (beroering.press.wasPressedThisFrame)
        {
            detRamte = Physics2D.OverlapPoint(worldPosition, traekbartLag);
        }

        // Finger holdes nede
        if (detRamte != null && beroering.press.isPressed)
        {
            detRamte.transform.position = worldPosition;
        }

        // Finger slippes
        if (beroering.press.wasReleasedThisFrame)
        {
            if (detRamte == null)
            {
                Debug.Log("detRamte er null");
            }

            if (detRamte != null)
            {
                //Veggie veggie = detRamte.GetComponent<Veggie>();
                //veggie.TjekOmKorrektPlads();
            }
        }
    }
}
