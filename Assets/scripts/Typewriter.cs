using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Typewriter : MonoBehaviour {

	public AudioClip typeWriter;
	private AudioSource audio;
	private TextMesh textMesh;
	private string text; 
	private float timeDelay;
	private int i;
	// Use this for initialization
	void Start () {
		i = 0;
		timeDelay = 0.0f;
		audio = gameObject.GetComponent<AudioSource> ();
		textMesh = gameObject.GetComponent<TextMesh> ();
		text = textMesh.text;
		textMesh.text = "";
		audio.volume = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (i < text.Length) {
			if (timeDelay <= 0.0f) {
				audio.PlayOneShot (typeWriter);
				StartCoroutine (delay());
				timeDelay = 0.25f;
			} else {
				timeDelay -= Time.deltaTime;
			}
		}
	}

	IEnumerator delay()
	{
		yield return new WaitForSeconds(0.2f);
		textMesh.text += text [i];
		i++;
	}
}
