using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <Summary>
/// Creates a column of spheres based on initial starting position. 
/// Change: Added a randomizer to sphere sizes, position checker used 
/// so they don't mesh with each other. They are also deleteable.
/// </Summary>


public class SimpleSpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    [Tooltip("How many spheres to spawn")]
    public int numberOfSpheres = 5; // public => editable in the inspector

    [Tooltip("Starting position for the first sphere")]
    public Vector3 startPosition = new Vector3(0, 1, 0); // x,y,z

    [Tooltip("Spacing between spheres on the X axis")]
    public float spacing = 1f; // 'f' literal => float constant

    [Tooltip("Random Sphere Sizes")]
    public float minsize = 1;
    public float maxsize = 4;

    // Start is called before the first frame update
    void Start()
    {
        // [Header] and [Tooltip] are Unity attributes that improve the Inspector UI.
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.transform.position = new Vector3(0, -1, 0);

        float currentX = startPosition.x;

        for (int i = 0; i < numberOfSpheres; i++)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.AddComponent<Rigidbody>().useGravity = enabled;

            // Random size
            float randomSize = Random.Range(minsize, maxsize);
            sphere.transform.localScale = Vector3.one * randomSize;

            // Position sphere based on its size to avoid overlaps
            float radius = randomSize / 2f;
            sphere.transform.position = new Vector3(currentX + radius, startPosition.y, startPosition.z);

            // Update currentX for the next sphere
            currentX += randomSize + spacing;

            sphere.tag = "Deleteable";
        }
    }
}