using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
	static public bool goalMet = false;

	void OnTriggerEnter(Collider other)
	{
		//when trigger is hit by something
		//check to see if its a projectile
		if (other.gameObject.tag == "Projectile") 
		{
			Goal.goalMet = true;
			//also set alpha color to higher opacity
			Color c = renderer.material.color;
			c.a = 1;
			renderer.material.color = c;
		}
	}
}
