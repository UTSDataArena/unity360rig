using UnityEngine;
using System.Collections;

public class CameraMultiplier : MonoBehaviour {
    
    public Camera baseCamera;
    // default is 6, more cameras = smoother transition between cameras
    public int cameraCount = 6;
    
    public float rotationOffset = 210.0f; // 3.5 x fov of a camera (ie 3 x 60 + 30)
    public float viewportOffsetH = 0.0f; 
    public float viewportOffsetV = 0.0f;
    
    public float viewportHeight = 1f;

    // data arena dimensions
    public float screenHeight = 4.0f;
    public float screenRadius = 4.9f;
    
    public float viewerHeight = 1.2f;
    
    private Camera myCamera;
    private Camera[] cameras;
    
    private bool obChanged = false;
    
    public float farClip = 50.0f;
    
    public bool enableBaseCamera = false;
    public bool enableObliqueControls = false;

    public GameObject basePlane;

    public GameObject[] planes;
    
    void Awake() {
        myCamera = (baseCamera != null) ? baseCamera : GetComponent<Camera>();
    }
    
    void makeScreens() {
        
        cameras = new Camera[cameraCount];
        planes = new GameObject[cameraCount];
        
        // create cameras
        for (int i = 0; i < cameras.Length; i++) {
        
            // make plane
            GameObject plane = Object.Instantiate (basePlane, transform);
            plane.SetActive(true);
            plane.name = "Plane"+ i.ToString("D3");
            plane.transform.parent = transform;
            plane.transform.Rotate(0, 0, rotationOffset + (i * (360 / cameras.Length)));
            
            // figure out screen dimensions based on camera number and screen dimensions
            float screenWidth = 2f * Mathf.Sin(Mathf.PI * 1f/cameraCount) * screenRadius;
            float screenDist = Mathf.Cos(Mathf.PI * 1f/cameraCount) * screenRadius; 
            // edges of camera frustum on screen
            plane.transform.Translate(-Vector3.up * screenDist);
            plane.transform.Translate(Vector3.forward * viewerHeight);
            // plane is 10x0x10, scale based on screenHeight
            plane.transform.localScale = new Vector3(.1f * screenWidth,0f, 0.1f * screenHeight);
            planes[i] = plane;
            
            //camera pos and orientation
            GameObject go = new GameObject("Camera"+ i.ToString("D3"));
            go.AddComponent(typeof(Camera));
            go.transform.parent = transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.Rotate(0, rotationOffset + (i * (360 / cameras.Length)), 0);
            
            cameras[i] = go.GetComponent<Camera>();
            // set pretty background colours
            cameras[i].backgroundColor = new Color (
                myCamera.backgroundColor.r + .1f * Mathf.Sin(2f * Mathf.PI * i / (float) cameraCount),
                myCamera.backgroundColor.g + .1f * Mathf.Sin(2f * Mathf.PI * (i + (cameraCount / 3f)) / (float) cameraCount),
                myCamera.backgroundColor.b + .1f * Mathf.Sin(2f * Mathf.PI * (i + (2 * cameraCount / 3f)) / (float) cameraCount)
            );
            cameras[i].depth = -cameraCount + myCamera.depth + i;
            
            
            // set camera rect on screen
            cameras[i].rect = new Rect(
                (2f * (viewportOffsetH + (((float) i) / cameras.Length)) % 2) / 2f,
                viewportOffsetV,
                (float) (1f / cameras.Length),
                viewportHeight
                );

            // off-axis projection settings
            cameras[i].gameObject.AddComponent(typeof(Kooima));
            Kooima kooima = cameras [i].GetComponent<Kooima> ();
            if (kooima) { 
                kooima.projectionScreen = planes[i];
                kooima.estimateViewFrustum = false;
                kooima.setNearClipPlane = true;
                kooima.nearClipDistanceOffset = -0.01f;
            }// */
        }
        
    }
    
    // Use this for initialization
    void Start () {
        makeScreens();
        GetComponent<Camera>().enabled = enableBaseCamera;
    }
    
    // minimum 3
    void setCameraCount(int count) {
        if (count == cameraCount) {
            return;
        }
        
        // destroy old cameras and screens
        for (int i = 0; i < cameras.Length; i++) {
            Destroy (cameras[i].gameObject);
            Destroy (planes[i]);
        }
        
        if (count < 6) {
            count = 6;
        }
        
        cameraCount = count;
        makeScreens();
    }
    
    // Update is called once per frame

    void Update () {
        // less cameras per screen
        if (Input.GetKey (KeyCode.Minus) || Input.GetKey (KeyCode.KeypadMinus)) {
            setCameraCount(cameraCount - 6);
        }
        
        // more cameras per screen
        if (Input.GetKey (KeyCode.Equals) || Input.GetKey (KeyCode.KeypadPlus)) {
            setCameraCount(cameraCount + 6);
        }
        
        if (Input.GetKey (KeyCode.PageUp)) {
            viewerHeight += 0.1f;
            foreach (GameObject plane in planes) {
                plane.transform.Translate(Vector3.forward * 0.1f);
            }
        }
        
        if (Input.GetKey (KeyCode.PageDown)) {
            viewerHeight -= 0.1f;
            foreach (GameObject plane in planes) {
                plane.transform.Translate(Vector3.forward * -0.1f);
            }
        }
        
    }

}
