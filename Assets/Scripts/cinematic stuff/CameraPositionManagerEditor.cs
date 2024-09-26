using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraPositionManager))]
public class CameraPositionManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CameraPositionManager manager = (CameraPositionManager)target;

        if (GUILayout.Button("Add Current Scene View Camera Position"))
        {
            AddCurrentSceneViewCameraPosition(manager);
        }

        for (int i = 0; i < manager.cameraPositions.Count; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Camera Position " + (i + 1));
            if (GUILayout.Button("Remove"))
            {
                manager.cameraPositions.RemoveAt(i);
                break;
            }

            if (GUILayout.Button("Set as Start Point"))
            {
                manager.startingPointIndex = i;
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("box");
            manager.cameraPositions[i].position = EditorGUILayout.Vector3Field("Position", manager.cameraPositions[i].position);
            manager.cameraPositions[i].rotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Rotation", manager.cameraPositions[i].rotation.eulerAngles));
            manager.cameraPositions[i].moveSpeed = EditorGUILayout.Slider("Move Speed", manager.cameraPositions[i].moveSpeed, 0.1f, 5f);
            manager.cameraPositions[i].shakeMagnitudeX = EditorGUILayout.Slider("Shake Magnitude X", manager.cameraPositions[i].shakeMagnitudeX, 0.01f, 5f);
            manager.cameraPositions[i].shakeMagnitudeY = EditorGUILayout.Slider("Shake Magnitude Y", manager.cameraPositions[i].shakeMagnitudeY, 0.01f, 5f);
            manager.cameraPositions[i].shakeFrequency = EditorGUILayout.Slider("Shake Frequency", manager.cameraPositions[i].shakeFrequency, 1f, 30f);
            manager.cameraPositions[i].pointColor = EditorGUILayout.ColorField("Point Color", manager.cameraPositions[i].pointColor);
            manager.cameraPositions[i].pauseAtPoint = EditorGUILayout.Toggle("Pause at Point", manager.cameraPositions[i].pauseAtPoint);
            if (manager.cameraPositions[i].pauseAtPoint)
            {
                manager.cameraPositions[i].pauseDuration = EditorGUILayout.FloatField("Pause Duration", manager.cameraPositions[i].pauseDuration);
                manager.cameraPositions[i].applyShakeDuringPause = EditorGUILayout.Toggle("Apply Shake During Pause", manager.cameraPositions[i].applyShakeDuringPause);
            }
            manager.cameraPositions[i].applyEffect = EditorGUILayout.Toggle("Apply Effect", manager.cameraPositions[i].applyEffect);
            if (manager.cameraPositions[i].applyEffect)
            {
                manager.cameraPositions[i].effectScript = (MonoBehaviour)EditorGUILayout.ObjectField("Effect Script", manager.cameraPositions[i].effectScript, typeof(MonoBehaviour), true);
            }
            manager.cameraPositions[i].triggerEvent = EditorGUILayout.Toggle("Trigger Event", manager.cameraPositions[i].triggerEvent);
            if (manager.cameraPositions[i].triggerEvent)
            {
                manager.cameraPositions[i].eventTarget = (GameObject)EditorGUILayout.ObjectField("Event Target", manager.cameraPositions[i].eventTarget, typeof(GameObject), true);
                manager.cameraPositions[i].eventMethodName = EditorGUILayout.TextField("Event Method Name", manager.cameraPositions[i].eventMethodName);
            }

            
            manager.cameraPositions[i].playAnimation = EditorGUILayout.Toggle("Play Animation", manager.cameraPositions[i].playAnimation);
            if (manager.cameraPositions[i].playAnimation)
            {
                manager.cameraPositions[i].animationDelay = EditorGUILayout.FloatField("Animation Delay", manager.cameraPositions[i].animationDelay);
                manager.cameraPositions[i].animatorParameter = EditorGUILayout.IntField("Animator Parameter", manager.cameraPositions[i].animatorParameter);
                manager.cameraPositions[i].animator = (Animator)EditorGUILayout.ObjectField("Animator", manager.cameraPositions[i].animator, typeof(Animator), true);
            }

            
            manager.cameraPositions[i].shakeDuringTransition = EditorGUILayout.Toggle("Shake During Transition", manager.cameraPositions[i].shakeDuringTransition);

            
            if (GUILayout.Button("Set Main Camera to This Point"))
            {
                SetMainCameraToPoint(manager.cameraPositions[i]);
            }

            EditorGUILayout.EndVertical();
        }

        GUILayout.Space(20);

        
        if (GUILayout.Button("Save Camera Positions"))
        {
            string path = EditorUtility.SaveFilePanel("Save Camera Positions", "", "CameraPositions.json", "json");
            if (!string.IsNullOrEmpty(path))
            {
                manager.Save(path);
            }
        }

        if (GUILayout.Button("Load Camera Positions"))
        {
            string path = EditorUtility.OpenFilePanel("Load Camera Positions", "", "json");
            if (!string.IsNullOrEmpty(path))
            {
                manager.Load(path);
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(manager);
        }
    }

    private void AddCurrentSceneViewCameraPosition(CameraPositionManager manager)
    {
        if (SceneView.lastActiveSceneView != null)
        {
            Camera sceneCamera = SceneView.lastActiveSceneView.camera;
            CameraPositionManager.CameraPosition newPosition = new CameraPositionManager.CameraPosition
            {
                position = sceneCamera.transform.position,
                rotation = sceneCamera.transform.rotation
            };
            manager.cameraPositions.Add(newPosition);
        }
        else
        {
            Debug.LogWarning("No active Scene view camera found.");
        }
    }

    private void SetMainCameraToPoint(CameraPositionManager.CameraPosition point)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.transform.position = point.position;
            mainCamera.transform.rotation = point.rotation;
            SceneView.lastActiveSceneView.LookAt(point.position);
        }
        else
        {
            Debug.LogWarning("Main Camera not found.");
        }
    }
}
