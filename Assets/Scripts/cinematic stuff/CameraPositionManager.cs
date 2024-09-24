using System.Collections.Generic;
using UnityEngine;
using System.IO;

[ExecuteInEditMode]
public class CameraPositionManager : MonoBehaviour
{
    [System.Serializable]
    public class CameraPosition
    {
        public Vector3 position;
        public Quaternion rotation;
        public float moveSpeed = 1.0f;
        public float shakeMagnitudeX = 2f;
        public float shakeMagnitudeY = 2f;
        public float shakeFrequency = 10.0f;
        public Color pointColor = Color.white;
        public bool pauseAtPoint = false;
        public float pauseDuration = 0f;
        public bool applyShakeDuringPause = false;
        public bool applyEffect = false;
        public MonoBehaviour effectScript;
        public bool triggerEvent = false; // New field for triggering events
        public GameObject eventTarget; // Object to trigger the event on
        public string eventMethodName; // Method name to trigger

        // New fields for animation
        public bool playAnimation = false; // New field to trigger animation
        public float animationDelay = 0f; // Delay before starting animation
        public int animatorParameter = 0; // Parameter to set in animator
        public Animator animator; // Reference to the animator

        // New field for shake during transition
        public bool shakeDuringTransition = false; // Shake for the duration of the transition to the next point
    }

    public List<CameraPosition> cameraPositions = new List<CameraPosition>();
    public int startingPointIndex = 0;

    private void OnDrawGizmos()
    {
        if (cameraPositions.Count == 0) return;

        Gizmos.color = Color.white;
        for (int i = 0; i < cameraPositions.Count; i++)
        {
            var point = cameraPositions[i];

            // Draw the point
            Gizmos.color = point.pointColor;
            Gizmos.DrawSphere(point.position, 0.01f);

            // Draw the path to the next point
            if (i < cameraPositions.Count - 1)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(point.position, cameraPositions[i + 1].position);
            }

            // Draw the rotation direction
            Gizmos.color = Color.red;
            Vector3 forwardDirection = point.rotation * Vector3.forward;
            Gizmos.DrawLine(point.position, point.position + forwardDirection * 0.5f);
        }
    }

    public void Save(string filePath)
    {
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(filePath, json);
    }

    public void Load(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(json, this);
        }
        else
        {
            Debug.LogWarning("File not found: " + filePath);
        }
    }
}
