using UnityEngine;
using System.Collections;

public class FPSCam : MonoBehaviour {
	
	public float sensitivity = 0.02f;
	public bool mouselook = true;
	
	public float xSpeed = 250;
	public float ySpeed = 120;
	
	public float yMinLimit = -20;
	public float yMaxLimit = 80;
	
	private float x = 0;
	private float y = 0;
	
	private float prevX = 0;
	private float prevY = 0;
	
	// Use this for initialization
	void Start () {
		transform.localRotation = Quaternion.identity;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Level"), Input.GetAxis("Vertical")) * sensitivity);
		
		if (mouselook) {
	        x += (Input.mousePosition.x - prevX ) * xSpeed * 0.02f;
	        y -= (Input.mousePosition.y - prevY ) * ySpeed * 0.02f;
	 		
	 		y = MouseOrbit.ClampAngle(y, yMinLimit, yMaxLimit);
	 		       
	        transform.rotation = Quaternion.Euler(y, x, 0);
	
	        prevX = Input.mousePosition.x;
	        prevY = Input.mousePosition.y;
		}
	}
}
