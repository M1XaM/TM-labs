using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroVideoPlayer : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string nextSceneName = "GameScene"; // Change this to your main game scene name
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Text skipText;

    private void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        // Set camera background to black
        targetCamera.clearFlags = CameraClearFlags.SolidColor;
        targetCamera.backgroundColor = Color.black;

        // Set up video player
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane; // Changed to NearPlane for better visibility
        videoPlayer.targetCamera = targetCamera;
        videoPlayer.aspectRatio = VideoAspectRatio.FitVertically;
        videoPlayer.playOnAwake = true;
        videoPlayer.waitForFirstFrame = true;

        // Ensure the video is visible
        videoPlayer.targetCameraAlpha = 1f;

        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();

        // Set up skip text
        if (skipText != null)
        {
            skipText.text = "Press SPACE to skip";
            skipText.color = Color.white;
            skipText.alignment = TextAnchor.LowerLeft;
        }

        Debug.Log("Video Player initialized. Video clip: " + (videoPlayer.clip != null ? videoPlayer.clip.name : "null"));
        Debug.Log("Video dimensions: " + (videoPlayer.clip != null ? videoPlayer.clip.width + "x" + videoPlayer.clip.height : "null"));
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }

    private void Update()
    {
        // Allow skipping the video with Space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
} 