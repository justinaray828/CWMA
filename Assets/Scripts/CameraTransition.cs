using UnityEngine;

/// <summary>
/// CameraTransition requires a brainscene to be present in the scene as well as a cube with a render texture of the scene on
/// the main character. Use TransitionTime to set how long in seconds the transistion will take place.
///
/// To Call the transistion call ZoomIn() or ZoomOut()
///
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraTransition : MonoBehaviour
{
    [Tooltip("How far the camera will move for each frame")]
    public float transitionTime = 2f;

    [Header("Needed Brain Scene GameObjects")]
    public GameObject brainScene;

    public GameObject brainSceneCube;
    public bool enableBrainScene;

    // Serialized for testing purposes
    public bool zoomIn = true;

    public bool startZoom = false;

    private bool cameraIsZoomedIn = false;

    private Camera mainCamera;

    //Camera starting values
    private float cameraStartSize = 10.8f;

    private Vector3 cameraStartPosition;

    //Zoom in camera target values
    private float cameraZoomInSize = 2.66f;

    private Vector3 cameraZoomPosition;

    private float zoomTolerance = .01f; //Needed to account for how far Lerp is off
    public float brainSceneTransitionFadeStart;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        SetCameraPositions();
    }

    /// <summary>
    /// Sets transform positions for the two different states of the camera
    /// </summary>
    private void SetCameraPositions()
    {
        cameraStartPosition = new Vector3(0f, 0f, -10f);
        cameraZoomPosition = new Vector3(brainSceneCube.transform.position.x,
                                         brainSceneCube.transform.position.y,
                                         cameraStartPosition.z);
    }

    /// <summary>
    /// Call to zoom camera in and open brain room
    /// </summary>
    public void ZoomIn()
    {
        zoomIn = true;
        startZoom = true;
        SetCameraPositions();
    }

    /// <summary>
    /// Call to zoom camera out and close brain room
    /// </summary>
    public void ZoomOut()
    {
        zoomIn = false;
        startZoom = true;
        SetCameraPositions();
    }

    private void Update()
    {
        CameraZoom();
    }

    /// <summary>
    /// Handles camera zoom in or out
    /// </summary>
    private void CameraZoom()
    {
        if (startZoom)
        {
            ZoomCamera();
            MoveCamera();
            CameraStateUpdate();
            if (mainCamera.orthographicSize <= brainSceneTransitionFadeStart) ;
        }

        EnableBrainScene();
    }

    /// <summary>
    /// Moves the position of the camera into required place
    /// </summary>
    private void MoveCamera()
    {
        if (zoomIn)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraZoomPosition, transitionTime * Time.deltaTime);
        }
        else
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraStartPosition, transitionTime * Time.deltaTime);
        }
    }

    public float smoothTime = 0.3f;
    public float yVelocity = 0.3f;

    /// <summary>
    /// Adjust camera size to zoom in or out
    /// </summary>
    private void ZoomCamera()
    {
        if (zoomIn)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, cameraZoomInSize, transitionTime * Time.deltaTime);
            //mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, cameraZoomInSize, ref yVelocity, smoothTime);
        }
        else
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, cameraStartSize, transitionTime * Time.deltaTime);
            //mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, cameraStartSize, ref yVelocity, smoothTime);
        }
    }

    /// <summary>
    /// Checks if the Orthographic size is zoomed in or not to update the state of the transition
    /// </summary>
    private void CameraStateUpdate()
    {
        if (zoomIn)
        {
            if (mainCamera.orthographicSize <= cameraZoomInSize + zoomTolerance)
            {
                cameraIsZoomedIn = true;
                startZoom = false;
                //orthographicSize = cameraZoomInSize;
                //mainCamera.transform.position = cameraZoomPosition;
            }
        }
        else
        {
            cameraIsZoomedIn = false;

            if (mainCamera.orthographicSize >= cameraStartSize - zoomTolerance)
            {
                startZoom = false;
                //mainCamera.orthographicSize = cameraStartSize;
                //mainCamera.transform.position = cameraStartPosition;
            }
        }
    }

    /// <summary>
    /// Enables the brain scene
    /// This is where code needs to go to trigger animation of head opening up.
    /// </summary>
    private void EnableBrainScene()
    {
        if (cameraIsZoomedIn || enableBrainScene)
        {
            brainScene.SetActive(true);
            brainSceneCube.SetActive(true);
        }
        else
        {
            brainScene.SetActive(false);
            brainSceneCube.SetActive(false);
        }
    }

    public bool isCameraZoomedIn()
    {
        return cameraIsZoomedIn;
    }
}