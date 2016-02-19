using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileLine : MonoBehaviour {
	static public ProjectileLine S; //Singleton

	//fields set in unity
	public float minDist = 0.1f;
	public bool ____________________;

	//fields set dynamiclly
	public LineRenderer line;
	private GameObject _poi;
	public List<Vector3> points;

	void Awake()
	{
		S = this; // set the singleton
		//get reference to LineRenderer
		line = GetComponent<LineRenderer> ();
		//disable lineRenderer till needed
		line.enabled = false;
		//Initialize the poits list
		points = new List<Vector3> ();

	}

	//property / method acting as a field
	public GameObject poi
	{
		get
		{
			return (_poi);
		}
		set
		{
			_poi = value;
			if (_poi != null)
			{
				line.enabled = false;
				points = new List<Vector3>();
				AddPoint();
			}
		}

	}

	//method to clear the line
	public void Clear()
	{
		_poi = null;
		line.enabled = false;
		points = new List<Vector3> ();
	}

	public void AddPoint()
	{
		Vector3 pt = _poi.transform.position;
		if (points.Count > 0 && (pt - lastPoint).magnitude < minDist) {
			return;
		}
		if (points.Count == 0) {
			Vector3 launchPos = Slingshot.S.launchPoint.transform.position;
			Vector3 launchPosDiff = pt - launchPos;
			points.Add (pt + launchPosDiff);
			points.Add (pt);
			line.SetPosition (0, points [0]);
			line.SetPosition (1, points [1]);
			// enables line Renderer
			line.enabled = true;
		} else 
		{
			//normal behavior for adding points
			points.Add(pt);
			line.SetVertexCount(points.Count);
			line.SetPosition(points.Count - 1, lastPoint);
			line.enabled = true;

		}
	}

	//Returns location of newest point
	public Vector3 lastPoint
	{
		get
		{
			if (points == null)
			{
				return (Vector3.zero);
			}
			return (points[points.Count-1]);
		}
	}

		
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (poi == null) 
		{
			if(FollowCam.S.poi != null)
			{
				if(FollowCam.S.poi.tag == "Projectile")
				{
					poi = FollowCam.S.poi;
				}else{
					return;// exit if did not find poi
				}
			}else{
				return; //exit if did not find poi
			}
		}
		//add poi if found
		AddPoint ();
		if (poi.rigidbody.IsSleeping ()) 
		{
			poi = null;//clear poi wonce it is sleeping/ not active
		}

	}
}
