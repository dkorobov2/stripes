﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour {

	public GameObject transition;

	// Use this for initialization
	void Awake () {
		//if (GameManager.fade == true) {
		Instantiate (transition, Vector3.zero, Quaternion.identity, gameObject.transform.parent);
		//}
	}
}
