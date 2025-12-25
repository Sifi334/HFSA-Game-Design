using UnityEngine;

// September 24 -- Key Syntax overview is broadly used in game project.

/// <summary>
/// September 29, Debug Script
/// Change: Use model imported to check whether it is under map or not.
/// </summary>
 
public class DebugExample : MonoBehaviour
{
    // Requires attached Model - keep in mind for debugging.
    public Transform Model;
    public float logInterval = 2f;
    private float timer;

    void Start()
    {
        Debug.Log("DebugLog: Game Started!");
    }

    // Poor method, dont use update for time-based intervals.
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= logInterval)
        {
            timer = 0f;

            if (Model != null)
            {
                Debug.Log("DebugLog: Current Position: " + Model.position);

                if (Model.position.y < 0)
                {
                    Debug.LogWarning("DebugLog: Object is in nullszone");
                }
            }
        }
    }
}