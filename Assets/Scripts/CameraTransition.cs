using UnityEngine;

/// <summary>
/// CameraTransition requires a brainscene to be present in the scene as well as a cube with a render texture of the scene on
/// the main character. Use TransitionTime to set how long in seconds the transistion will take place.
///
/// To Call the transistion call ZoomIn() or ZoomOut()
///
/// ToggleBrainRoomCut() will cut between brainroom and scene
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraTransition : MonoBehaviour
{
    [Tooltip("How far the camera will move for each frame")]
    public float transitionTime = 2f;

    [Header("Needed Brain Scene GameObjects")]
    [Tooltip("Found under GameManager")]
    public GameObject brainScene;

    [Tooltip("Found under BrainScene")]
    public GameObject brainCameraCut;

    [Tooltip("Found under Main Character")]
    public GameObject brainSceneCube;

    [Tooltip("Found under Main Character")]
    public Animator brainOpenAnimator;

    //private bool enableBrainScene;
    private bool zoomIn = true;

    public bool startZoom = false;
    private bool cameraIsZoomedIn = false;
    private bool turnCameraOn = false; //Used for ToggleBrainRoomCut()

    private Camera mainCamera;

    private float cameraStartSize = 10.8f;
    private float cameraZoomInSize = 2.66f;
    private float zoomTolerance = .01f; //Needed to account for how far Lerp is off
    private float smoothTime = 0.3f;
    private float yVelocity = 0.3f;
    private float cameraStatPositionX = 0;
    private float cameraStatPositionY = 0;
    private float cameraStatPositionZ = -10;

    private Vector3 cameraStartPosition;
    private Vector3 cameraZoomPosition;

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
        cameraStartPosition = new Vector3(cameraStatPositionX, cameraStatPositionY, cameraStatPositionZ);

        if (brainSceneCube)
        {
            cameraZoomPosition = new Vector3(brainSceneCube.transform.position.x,
                                             brainSceneCube.transform.position.y,
                                             cameraStartPosition.z);
        }
    }

    /// <summary>
    /// Call to zoom camera in and open brain room
    /// </summary>
    public void ZoomIn()
    {
        GetComponent<Animator>().enabled = false;
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
        if (!turnCameraOn) //Check if camera is currently cut to brainroom
            CameraZoom();
    }

    /// <summary>
    /// Handles camera zoom in or out
    /// </summary>
    private void CameraZoom()
    {
        if (startZoom)
        {
            MoveAndZoomCamera();
            CameraStateUpdate();
        }
    }

    /// <summary>
    /// Moves the position of the camera into required place
    /// </summary>
    private void MoveAndZoomCamera()
    {
        if (zoomIn)
        {
            EnableBrainScene(true);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraZoomPosition, transitionTime * Time.deltaTime);
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, cameraZoomInSize, transitionTime * Time.deltaTime);
        }
        else //Zoomout
        {
            EnableBrainScene(false);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraStartPosition, transitionTime * Time.deltaTime);
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, cameraStartSize, transitionTime * Time.deltaTime);
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
            }
        }
        else
        {
            cameraIsZoomedIn = false;

            if (mainCamera.orthographicSize >= cameraStartSize - zoomTolerance)
            {
                GetComponent<Animator>().enabled = true;
                startZoom = false;
            }
        }
    }

    /// <summary>
    /// Enables the brain scene
    /// This is where code needs to go to trigger animation of head opening up.
    /// </summary>
    private void EnableBrainScene(bool boolean)
    {
        if (brainOpenAnimator)
        {
            brainOpenAnimator.SetBool("Open", boolean);
        }
    }

    /// <summary>
    /// Toggle between brainroom cut and scene
    /// </summary>
    public void ToggleBrainRoomCut()
    {
        turnCameraOn = !turnCameraOn;
        brainScene.SetActive(turnCameraOn);
        brainCameraCut.SetActive(turnCameraOn);
        if (brainSceneCube)
        {
            brainSceneCube.SetActive(!turnCameraOn);
        }
    }

    public bool isCameraZoomedIn()
    {
        return cameraIsZoomedIn;
    }
}