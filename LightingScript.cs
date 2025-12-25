using UnityEngine;
using UnityEngine.Rendering;

/// <Summary>
/// Lighting Script using Advanced + B-D Example
/// Handles Base Lighting and Pulsing boolean.
/// Change: Fusing all scripts into one compact implementation, Different values.
/// </Summary>

public class TryLightGlobalScript : MonoBehaviour
{
    Light directionalLight;

    public float baseIntensity = 1f;
    public float pulseAmount = 0.5f;
    public float pulseSpeed = 2f;
    public bool enablePulsing = true;

    Renderer rend;

    // Using Mostly Editor-Friendly Setup 
    void Start()
    {
        GameObject go = new GameObject("Directional Light");
        directionalLight = go.AddComponent<Light>();
        directionalLight.type = LightType.Directional;
        directionalLight.color = Color.red;
        directionalLight.intensity = 5f;
        directionalLight.shadows = LightShadows.Soft;
        go.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        rend = GetComponent<Renderer>();
    }
    void Update()
    {
        // Pulse
        if (enablePulsing)
        {
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) * 0.5f + 0.5f);
            directionalLight.intensity = baseIntensity + pulse * pulseAmount;
        }

        // Probe sampling
        if (rend != null)
        {
            LightProbes.GetInterpolatedProbe(transform.position, rend, out SphericalHarmonicsL2 probe);
        }
    }

    // Baketype functions
    public void SetRealtime()
    {
        directionalLight.lightmapBakeType = LightmapBakeType.Realtime;
    }

    public void SetBaked()
    {
        directionalLight.lightmapBakeType = LightmapBakeType.Baked;
    }

    public void SetMixed()
    {
        directionalLight.lightmapBakeType = LightmapBakeType.Mixed;
    }
}
