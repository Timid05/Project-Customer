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
            currentTargetIndex = positionManager.startingPointIndex;
            transform.position = positionManager.cameraPositions[currentTargetIndex].position;
            transform.rotation = positionManager.cameraPositions[currentTargetIndex].rotation;

            SetupNextJourney();
        }
    }

    void Update()
    {
        if (!isMoving || isPaused || positionManager == null || positionManager.cameraPositions.Count == 0) return;

        CameraPositionManager.CameraPosition target = positionManager.cameraPositions[currentTargetIndex];

   
        float distanceCovered = (Time.time - startTime) * target.moveSpeed;
        float fractionOfJourney = distanceCovered / journeyLength;

        transform.position = Vector3.Lerp(startPosition, target.position, fractionOfJourney);

     
        transform.rotation = Quaternion.Slerp(startRotation, target.rotation, fractionOfJourney);

        if (target.shakeDuringTransition && fractionOfJourney < 1f)
        {
            ApplyCameraShake(target);
            //play footsteps
        }

        
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

         
            if (target.triggerEvent && target.eventTarget != null && !string.IsNullOrEmpty(target.eventMethodName))
            {
                target.eventTarget.SendMessage(target.eventMethodName, SendMessageOptions.DontRequireReceiver);
            }

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
            isMoving = false; 
        }
        else
        {
        
            SetupNextJourney();

            
            CameraPositionManager.CameraPosition target = positionManager.cameraPositions[currentTargetIndex];
            if (target.applyEffect && target.effectScript != null)
            {
                target.effectScript.Invoke("ExecuteEffect", 0f);
            }

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
        journeyLength = Vector3.Distance(transform.position, target.position) / (target.moveSpeed * 0.5f); 
        startPosition = transform.position;
        startRotation = transform.rotation;
    }
}
