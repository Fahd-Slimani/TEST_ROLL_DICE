using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dice : Singleton<Dice>
{
    private Rigidbody m_Rigidbody;
    private bool rolling = false;
    private float rollDuration = 1.0f;
    private float rollStartTime;
    private bool snapping = false;
    private Quaternion targetRotation;

    public float snapSpeed = 10f;
    public float rollingSpeed = 12;

    // Event triggered when the dice stops rolling
    public Action<int> OnDiceRolled;

    private int diceResult;
    public int lastDiceRoll;

    // Get the Rigidbody component at the start
    void Start() => m_Rigidbody = GetComponent<Rigidbody>();

    // Starts rolling the dice
    public void RollDice()
    {
        if (!rolling)
        {
            rolling = true;
            rollStartTime = Time.time;
            m_Rigidbody.angularVelocity = UnityEngine.Random.insideUnitSphere * rollingSpeed; // Apply random spin
        }
    }

    private void FixedUpdate()
    {
        // Check if the dice has been rolling long enough
        if (rolling)
        {
            if (Time.time - rollStartTime >= rollDuration)
            {
                rolling = false;
                m_Rigidbody.angularVelocity = Vector3.zero; // Stop spinning
                SnapToNearestFace();
            }
        }

        // Smoothly adjust the dice to the nearest face
        if (snapping)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * snapSpeed);

            // If the rotation is close enough, finalize it
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.5f)
            {
                transform.rotation = targetRotation;
                snapping = false;
                IdentifyDiceResult();
            }
        }
    }

    // Finds and aligns the dice to the nearest valid face
    private void SnapToNearestFace()
    {
        Vector3[] possibleRotations =
        {
            Vector3.right * 90, Vector3.left * 90,
            Vector3.up * 90, Vector3.down * 90,
            Vector3.forward * 90, Vector3.back * 90
        };

        Quaternion closestRotation = Quaternion.identity;
        float closestAngle = float.MaxValue;

        // Find the closest valid face rotation
        foreach (var rotation in possibleRotations)
        {
            Quaternion rot = Quaternion.Euler(rotation);
            float angle = Quaternion.Angle(transform.rotation, rot);
            if (angle < closestAngle)
            {
                closestAngle = angle;
                closestRotation = rot;
            }
        }

        targetRotation = closestRotation;
        snapping = true;
    }

    // Identifies the top face of the dice and gets the result
    private void IdentifyDiceResult()
    {
        RaycastHit hit;

        // Cast a ray downward to check which number is facing up
        if (Physics.Raycast(transform.position, Vector3.back, out hit, 100f))
        {
            diceResult = int.Parse(hit.collider.gameObject.name);

            OnDiceRolled?.Invoke(diceResult); // Notify listeners about the result

            lastDiceRoll = diceResult; // Store the last roll result
        }
    }
}
