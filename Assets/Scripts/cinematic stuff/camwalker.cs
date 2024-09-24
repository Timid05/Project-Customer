using System.Collections;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public CameraPositionManager positionManager;
    private int currentTargetIndex;
    private bool isMoving = true;
    private bool isPaused = false;
    private float shakeTimer = 0.0f;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private float journeyLength;
    private float startTime;

    void Start()
    {
        if (positionManager != null && positionManager.cameraPositions.Count > 0)
        {
            // Set the starting point
            currentTargetIndex = positionManager.startingPointIndex;
            // Set initial position and rotation
            transform.position = positionManager.cameraPositions[currentTargetIndex].position;
            transform.rotation = positionManager.cameraPositions[currentTargetIndex].rotation;

            // Initialize journey parameters
            SetupNextJourney();
        }
    }

    void Update()
    {
        if (!isMoving || isPaused || positionManager == null || positionManager.cameraPositions.Count == 0) return;

        CameraPositionManager.CameraPosition target = positionManager.cameraPositions[currentTargetIndex];

        // Calculate the fraction of the journey completed
        float distanceCovered = (Time.time - startTime) * target.moveSpeed;
        float fractionOfJourney = distanceCovered / journeyLength;

        // Move towards the target position
        transform.position = Vector3.Lerp(startPosition, target.position, fractionOfJourney);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(startRotation, target.rotation, fractionOfJourney);

        // Apply camera shake while moving
        if (target.shakeDuringTransition && fractionOfJourney < 1f)
        {
            ApplyCameraShake(target);
        }

        // Check if the camera has reached the target position and rotation
        if (fractionOfJourney >= 1f)
        {
            if (target.pauseAtPoint)
            {
                StartCoroutine(Pause(target));
            }
            else
            {
                NextTarget();
            }

            // Trigger event if enabled
            if (target.triggerEvent && target.eventTarget != null && !string.IsNullOrEmpty(target.eventMethodName))
            {
                target.eventTarget.SendMessage(target.eventMethodName, SendMessageOptions.DontRequireReceiver);
            }

            // Play animation if enabled
            if (target.playAnimation && target.animator != null )
            {
                StartCoroutine(PlayAnimation(target));
            }
        }
    }

    private void ApplyCameraShake(CameraPositionManager.CameraPosition target)
    {
        shakeTimer += Time.deltaTime * target.shakeFrequency;
        float shakeOffsetX = Mathf.Sin(shakeTimer) * target.shakeMagnitudeX;
        float shakeOffsetY = Mathf.Cos(shakeTimer) * target.shakeMagnitudeY;
        Vector3 shakeOffset = new Vector3(shakeOffsetX, shakeOffsetY, 0);

        // Apply the shake effect temporarily
        transform.rotation *= Quaternion.Euler(shakeOffset);
    }

    private IEnumerator Pause(CameraPositionManager.CameraPosition target)
    {
        isPaused = true;

        float pauseTimer = 0.0f;
        while (pauseTimer < target.pauseDuration)
        {
            if (target.applyShakeDuringPause)
            {
                ApplyCameraShake(target);
            }
            pauseTimer += Time.deltaTime;
            yield return null;
        }

        isPaused = false;
        NextTarget();
    }

    private IEnumerator PlayAnimation(CameraPositionManager.CameraPosition target)
    {
        yield return new WaitForSeconds(target.animationDelay);
        target.animator.SetInteger("State", target.animatorParameter);
    }

    private void NextTarget()
    {
        currentTargetIndex++;

        if (currentTargetIndex >= positionManager.cameraPositions.Count)
        {
            isMoving = false; // Stop moving after the last point
        }
        else
        {
            // Update journey parameters for the next target
            SetupNextJourney();

            // Execute the effect script if it's assigned
            CameraPositionManager.CameraPosition target = positionManager.cameraPositions[currentTargetIndex];
            if (target.applyEffect && target.effectScript != null)
            {
                target.effectScript.Invoke("ExecuteEffect", 0f);
            }

            // Trigger event if enabled
            if (target.triggerEvent && target.eventTarget != null && !string.IsNullOrEmpty(target.eventMethodName))
            {
                target.eventTarget.SendMessage(target.eventMethodName, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private void SetupNextJourney()
    {
        startTime = Time.time;
        CameraPositionManager.CameraPosition target = positionManager.cameraPositions[currentTargetIndex];
        journeyLength = Vector3.Distance(transform.position, target.position) / (target.moveSpeed * 0.5f); // Lower overall speed
        startPosition = transform.position;
        startRotation = transform.rotation;
    }
}
