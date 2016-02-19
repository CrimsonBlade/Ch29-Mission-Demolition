using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
	static public FollowCam S; //a FollowCam Singleton

	//fields set in unity
	public float easing = 0.05f;
	public Vector2 minXY;
	public bool ___________________________;

	//fields set dynamiclly
	public GameObject poi;//the point of interest
	public float camZ; //the desired Z pos of the camera

	void Awake()
	{
		S = this;
		camZ = this.transform.position.z;
	}


	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 destination;
		//return to start if no poi
		if (poi == null) {
			destination = Vector3.zero;
		} else {
			//get position of poi
			destination = poi.transform.position;
			//if projectile is it at rest
			if(poi.tag == "Projectile"){
				//is it moving
				if(poi.rigidbody.IsSleeping()){
					//retuen to default view
					poi = null;
					//in the next update
					return;
				}
			}
		}

		//if (poi == null) return; //return if no poi
		// get position of poi
		//Vector3 destination = poi.transform.position;
		//Limit the x & y values
		destination.x = Mathf.Max (minXY.x, destination.x);
		destination.y = Mathf.Max (minXY.y, destination.y);

		//Lerp from start to destination by factor of easing
		destination = Vector3.Lerp (transform.position, destination, easing);
		//retain a destination.z of camZ
		destination.z = camZ;
		//set the camera to destination
		transform.position = destination;
		//adgust camera to enlarge as projectilegets higher
		this.camera.orthographicSize = destination.y + 10;
	}
}
