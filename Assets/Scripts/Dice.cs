using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dice : Singleton<Dice>
{
    private Rigidbody rb;
    private bool rolling = false;
    private float rollDuration = 1.0f;
    private float rollStartTime;
    private bool snapping = false;
    private Quaternion targetRotation;

    public float snapSpeed = 10f;
    public float rollingSpeed = 12;

    // triggered when the dice ends rolling
    public Action<int> OnDiceRolled;

    private int diceResult;
    public int lastDiceRoll;

    void Start() => rb = GetComponent<Rigidbody>();

    public void RollDice()
    {
        if (!rolling)
        {
            rolling = true;
            rollStartTime = Time.time;
            rb.angularVelocity = UnityEngine.Random.insideUnitSphere * rollingSpeed; // Apply random rotation speed
        }
    }

    private void FixedUpdate()
    {
        if (rolling)
        {
            if (Time.time - rollStartTime >= rollDuration)
            {
                rolling = false;
                rb.angularVelocity = Vector3.zero; // Stop rotation
                SnapToNearestFace();
            }
        }

        if (snapping)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * snapSpeed);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.5f)
            {
                transform.rotation = targetRotation;
                snapping = false;
                IdentifyDiceResult();
            }
        }
    }

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

    private void IdentifyDiceResult()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.back, out hit, 100f))
        {
            diceResult = int.Parse(hit.collider.gameObject.name);
            OnDiceRolled?.Invoke(diceResult);  // Only invoke if there's at least one listener

            lastDiceRoll = diceResult; // Store the last dice roll
        }
    }
}
