using UnityEngine;

/// <Summary>
/// Moves using Mouse down and raycasting. Not a bad implementation for a mobile game.
/// Change: Nothing really, Camera is public rather than found automatically.
/// Given the code obtains it's objective, I would love to not Change anything tbh.
/// </Summary>
public class ObjectMover : MonoBehaviour
{
    public float moveSpeed = 5f;
    // Don't make camera find itself, better to just import from heirarchy.
    public Camera mainCamera;
    private bool isDragging = false;

    void Update()
    {
        HandleKeyboardMovement();
        HandleMouseMovement();
    }

    // Charactercollider implementation.
    void HandleKeyboardMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0f, moveZ) * moveSpeed * Time.deltaTime;
        transform.position += movement;
    }

    void HandleMouseMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            if (groundPlane.Raycast(ray,out float distance))
            {
                Vector3 targetPoint = ray.GetPoint(distance);
                transform.position = Vector3.Lerp(transform.position, targetPoint, Time.deltaTime * moveSpeed);
            }
        }
    }
}
