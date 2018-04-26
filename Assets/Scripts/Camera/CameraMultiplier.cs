using UnityEngine;
using System.Collections;

public class CameraMultiplier : MonoBehaviour {
    
    public Camera baseCamera;
    // default is 6, more cameras = smoother transition between cameras
    public int cameraCount = 6;
    
    public float rotationOffset = 210.0f; // 3.5 x fov of a camera (ie 3 x 60 + 30)
    public float viewportOffsetH = 0.0f; 
    public float viewportOffsetV = 0.3f;
    
    public float viewportHeight = 0.5f;
    
    public float hOblique = 1.0f;
    public float vOblique = -1.0f;
    
    private Camera myCamera;
    private Camera[] cameras;
    
    private bool obChanged = false;
    
    public float farClip = 50.0f;
    
    public bool enableBaseCamera = false;
    public bool enableObliqueControls = false;
    
    static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far) {
        float x = 2.0F * near / (right - left);
        float y = 2.0F * near / (top - bottom);
        float a = (right + left) / (right - left);
        float b = (top + bottom) / (top - bottom);
        float c = -(far + near) / (far - near);
        float d = -(2.0F * far * near) / (far - near);
        float e = -1.0F;
        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = x;
        m[0, 1] = 0;
        m[0, 2] = a;
        m[0, 3] = 0;
        m[1, 0] = 0;
        m[1, 1] = y;
        m[1, 2] = b;
        m[1, 3] = 0;
        m[2, 0] = 0;
        m[2, 1] = 0;
        m[2, 2] = c;
        m[2, 3] = d;
        m[3, 0] = 0;
        m[3, 1] = 0;
        m[3, 2] = e;
        m[3, 3] = 0;
        return m;
    }
    
    void Awake() {
        myCamera = (baseCamera != null) ? baseCamera : GetComponent<Camera>();
    }
    
    // return the fov for the other dimension given an aspect ratio
    private float hFovGiven(float vFov, float aspect) {
        return Mathf.Rad2Deg * 2f * Mathf.Atan(Mathf.Tan(Mathf.Deg2Rad * vFov /2f) * aspect);    
    }
    
    // Use this for initialization
    void Start () {
        
        cameras = new Camera[cameraCount];
        for (int i = 0; i < cameras.Length; i++) {
            GameObject go = new GameObject("Camera"+ i.ToString("D3"));
            go.AddComponent(typeof(Camera));
            go.transform.parent = transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            // orient camera
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
            
            cameras[i].aspect = (1f / cameras.Length) / viewportHeight;
            
            // given that unity scales hfov based on aspect ratio, 
            // find the required vfov given the aspect ratio of the cameras and expected hfov
            cameras[i].fieldOfView = hFovGiven(360f / ((float) cameras.Length), 1f / cameras[i].aspect);
            
            
            cameras[i].farClipPlane = farClip;
        }
        
        GetComponent<Camera>().enabled = enableBaseCamera;
    }
    
    // Update is called once per frame
    void LateUpdate () {

        // reset obliqueness
        // changing the obliqueness moves the camera's 'center', allowing for off-axis projection
        if (Input.GetKey(KeyCode.R)) {
            hOblique = vOblique = 0f;
        }
        if (!enableObliqueControls) return;
        if (Input.GetKey(KeyCode.DownArrow)) {
            vOblique -= 0.01f;
            obChanged = true;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            vOblique += 0.01f;
            obChanged = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            hOblique += 0.01f;
            obChanged = true;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            hOblique -= 0.01f;
            obChanged = true;
        }
                    
        if (obChanged) {
            foreach (Camera cam in cameras) {
                Matrix4x4 m = cam.projectionMatrix;
                m[0, 2] = hOblique;
                m[1, 2] = vOblique;
                cam.projectionMatrix = m;
            }
            
            obChanged = false;
        }
    }
}
