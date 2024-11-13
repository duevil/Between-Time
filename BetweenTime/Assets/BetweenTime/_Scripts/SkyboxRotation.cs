using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    public float rotationSpeed;  // Geschwindigkeit der Rotation

    void Update()
    {
        // Skybox mit der Zeit rotieren
        float rotation = Time.time * rotationSpeed; 
        // Rotation entlang der Y-Achse
        RenderSettings.skybox.SetFloat("_Rotation", rotation);
    }
}
