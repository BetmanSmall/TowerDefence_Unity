using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour {
	Camera mycam;
	float speed = 5f;
	void Update () {
		Camera mycam = GetComponent<Camera>();
        float sensitivity = 0.05f;
		Vector3 vp = mycam.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mycam.nearClipPlane));
		vp.x -= 0.5f;
		vp.y -= 0.5f;
		vp.x *= sensitivity;
		vp.y *= sensitivity;
		vp.x += 0.5f;
		vp.y += 0.5f;
		Vector3 sp = mycam.ViewportToScreenPoint(vp);
		 
		Vector3 v = mycam.ScreenToWorldPoint(sp);
		transform.LookAt(v, Vector3.up);

		if(Input.GetKey(KeyCode.RightArrow))
		{
			transform.Translate(new Vector3(speed * Time.deltaTime,0,0));
		}
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			transform.Translate(new Vector3(-speed * Time.deltaTime,0,0));
		}
		if(Input.GetKey(KeyCode.DownArrow))
		{
			transform.Translate(new Vector3(0,-speed * Time.deltaTime,0));
		}
		if(Input.GetKey(KeyCode.UpArrow))
		{
			transform.Translate(new Vector3(0,speed * Time.deltaTime,0));
		}
		
	}
}
