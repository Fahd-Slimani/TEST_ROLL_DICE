using System.Collections;
using UnityEngine;

public class ShakingBehaviour : MonoBehaviour
{
    public float shakeDuration = 1f;  
    public float shakeIntensity = 0.1f; 

    private Vector3 originalPosition; // Store the original position

    // We shake the dice position without affecting its rotation
    public void ShakeDice()
    {
        StopAllCoroutines(); // Make sur to stop any existing shake before starting a new one
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        originalPosition = transform.position; // Store the original position

        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Apply random position movement (but no rotation)
            Vector3 randomOffset = new Vector3(
                Random.Range(-shakeIntensity, shakeIntensity),
                Random.Range(-shakeIntensity, shakeIntensity),
                Random.Range(-shakeIntensity, shakeIntensity)
            );

            transform.position = originalPosition + randomOffset;

            yield return null; // Wait for next frame
        }

        // Reset to the original position
        transform.position = originalPosition;
    }
}
