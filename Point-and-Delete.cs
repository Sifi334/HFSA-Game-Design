using UnityEngine;
using UnityEngine.InputSystem;    // This is using New Input system (?)
using UnityEngine.EventSystems;   // EventSystem for UI
using UnityEngine.InputSystem.UI; // Pointerid


/// <Summary>
/// Point and Delete function September 16th.
/// Change: Using AI to debug the original code it was able to decipher
/// MouseId can be switched due to mouse Pointer Id always being 0.
/// Touch was also minutely updated, I'm guessing the initial implementation
/// Was able to use Touch and PointerID codependently, but I had an error with my implementation
/// Requiring debugging and change. The current implementation works when an object is
/// Given Deleteable Tag. 
/// </Summary>
public class DeleteOnClickSafe : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera cam; //  If null, falls back to Camera.main

    [Header("Filters (configure these)")]
    [SerializeField] private LayerMask deleteableMask = 0; // Safe Default 0, nothing.
    [SerializeField] private bool requireTag = true; // Default: true
    [SerializeField] private string requiredTag = "Deleteable";

    [Header("Behavior")]
    [SerializeField] private bool ignoreWhenOverUI = true; // UI Prioritized to prevent deletion
    [SerializeField] private float maxDistance = 500f; // Raycast distance cap
    [SerializeField] private bool softDelete = true; // Will setActive(false) instead of destroy

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) Debug.LogError("DeleteOnClickSafe: No camera assigned and no Camera.main found.");
    }

    private void Update()
    {
        // Main Change here
        int mousePointerId = 0;

        // Mouse Input
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {   // Change to MousePointerId from Pointerid. ...
            if (!(ignoreWhenOverUI && IsPointerOverUI(mousePointerId)))
            TryDeleteAt(Mouse.current.position.ReadValue());
        }

        // Touch input
        if (Touchscreen.current != null)
        {
            foreach (var touch in Touchscreen.current.touches)
            {
                if (touch.press.wasPressedThisFrame)
                {
                    int pid = (int)touch.touchId.ReadValue(); // unique ID per touch
                    if (!(ignoreWhenOverUI && IsPointerOverUI(pid)))
                        TryDeleteAt(touch.position.ReadValue());
                }
            }
        }

        // Keyboard + Mouse click
        if (Keyboard.current != null &&
            (Keyboard.current.deleteKey.wasPressedThisFrame || Keyboard.current.backspaceKey.wasPressedThisFrame) &&
            Mouse.current != null)
        {
            if (!(ignoreWhenOverUI && IsPointerOverUI(0))) 
                TryDeleteAt(Mouse.current.position.ReadValue());
        }
    }

    private bool IsPointerOverUI(int pointerId)
    {
        if (EventSystem.current == null) return false;
        if (EventSystem.current.IsPointerOverGameObject(pointerId)) return true;

        return EventSystem.current.IsPointerOverGameObject(); // fallback(?)
    }

    private int GetTouchPointerId(UnityEngine.InputSystem.Controls.TouchControl touch)
    {
        try{ return (int)touch.touchId.ReadValue(); }
        catch { return 0; }
    }

    private void TryDeleteAt(Vector2 screenPos)
    {
        if (cam == null) return;
        Ray ray = cam.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, deleteableMask, QueryTriggerInteraction.Ignore))
        {
            var go = hit.collider.gameObject;

            // Never delete the host Camera
            if (go == cam.gameObject) return;

            if (requireTag && !go.CompareTag(requiredTag)) return;
            
            if (softDelete){
                go.SetActive(false);
            }
            else
            {
                Destroy(go);
            }
        }
    }


}
