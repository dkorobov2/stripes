using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickPoly;

public class Spikes : MonoBehaviour {
	public GameObject spike;

	public bool spikesBottom, spikesTop, spikesLeft, spikesRight;
	public int numSpikesHorizontal;

	private int numSpikesVertical;

	private float leftBarrier, rightBarrier, topBarrier, bottomBarrier;
	private float offset;

	// Use this for initialization
	void Awake () {
		Vector2 topRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
		Vector2 bottomRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 0));
		Vector2 topLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 1));
		Vector2 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

		float padding = 0.14f;

		leftBarrier = topLeft.x;
		rightBarrier = bottomRight.x;
		topBarrier = topRight.y;
		bottomBarrier = bottomRight.y;

		numSpikesVertical = Mathf.RoundToInt(numSpikesHorizontal * topBarrier / rightBarrier);
		Debug.Log (numSpikesVertical);
		Vector3 position;

		if (spikesBottom) 
		{
			offset = (rightBarrier - leftBarrier - 2*padding) / numSpikesHorizontal;
			spike.GetComponent<QuickPolygon> ().SetWidth (offset + padding);
			spike.GetComponent<QuickPolygon> ().SetHeight (offset + padding/2);
			for (var i = 0; i < numSpikesHorizontal; i++) {
				position.x = padding + leftBarrier + (offset/2) + i * offset;
				position.y = bottomBarrier - padding;
				position.z = 0;
				Instantiate (spike, position, Quaternion.identity, this.transform);
			}
		}

		if (spikesTop) 
		{
			Quaternion rotation = Quaternion.Euler (0, 0, 180);
			offset = (rightBarrier - leftBarrier - 2*padding) / numSpikesHorizontal;
			spike.GetComponent<QuickPolygon> ().SetWidth (offset + padding);
			spike.GetComponent<QuickPolygon> ().SetHeight (offset + padding/2);
			for (var i = 0; i < numSpikesHorizontal; i++) {
				position.x = padding + leftBarrier + (offset/2) + i * offset;
				position.y = topBarrier + padding;
				position.z = 0;
				Instantiate (spike, position, rotation, this.transform);
			}
		}

		if (spikesLeft) 
		{
			Quaternion rotation = Quaternion.Euler (0, 0, -90);
			offset = (topBarrier - bottomBarrier - 2*padding) / numSpikesVertical;
			spike.GetComponent<QuickPolygon> ().SetWidth (offset + padding);
			spike.GetComponent<QuickPolygon> ().SetHeight (offset + padding/2);
			for (var i = 0; i < numSpikesVertical; i++) {
				position.x = leftBarrier - padding;
				position.y = padding + bottomBarrier  + (offset/2) + i * offset;
				position.z = 0;
				/*
				if (spikesBottom && i == 0)
					continue;
				else if (spikesTop && i == numSpikesVertical - 1)
					continue;
				*/
				Instantiate (spike, position, rotation, this.transform);
			}
		}
		if (spikesRight) 
		{
			Quaternion rotation = Quaternion.Euler (0, 0, 90);
			offset = (topBarrier - bottomBarrier - 2*padding) / numSpikesVertical;
			spike.GetComponent<QuickPolygon> ().SetWidth (offset + padding);
			spike.GetComponent<QuickPolygon> ().SetHeight (offset + padding/2);
			for (var i = 0; i < numSpikesVertical; i++) {
				position.x = rightBarrier + padding;
				position.y = padding + bottomBarrier  + (offset/2) + i * offset;
				position.z = 0;
				/*
				if (spikesBottom && i == 0)
					continue;
				else if (spikesTop && i == numSpikesVertical - 1)
					continue;
				*/
				Instantiate (spike, position, rotation, this.transform);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
