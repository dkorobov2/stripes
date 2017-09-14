using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour {

	AudioSource audioSource;

	public Sprite sound_on;
	public Sprite sound_off;
	public GameObject sound_image;

	// Use this for initialization
	void Start () {
		audioSource = GameObject.Find("audio").GetComponent<AudioSource>();

		if (GameManager.sound == true) {
			sound_image.GetComponent<SpriteRenderer> ().sprite = sound_on;
		}
		else if (GameManager.sound == false) {
			sound_image.GetComponent<SpriteRenderer> ().sprite = sound_off;
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit2D hit;
			try {
				hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
			} catch (System.NullReferenceException) {
				return;
			}

			if (hit.transform == null)
				return;

			Debug.Log (hit.transform.name);
			//hit.transform.gameObject.transform.parent.GetComponent<Animator>().enabled = true;

			string button = hit.transform.name;

			if (button != "sound" && button != "unlock" && button != "erase" && button != "rate")
				return;

			if (button == "sound") {
				if (GameManager.sound == true) {
					GameManager.updateSound (false);
					sound_image.GetComponent<SpriteRenderer> ().sprite = sound_off;
				} else if (GameManager.sound == false) {
					GameManager.updateSound (true);
					sound_image.GetComponent<SpriteRenderer> ().sprite = sound_on;
				}
				//button = GameManager.maxLevel.ToString ();
			} else if (button == "erase") {
				GameManager.resetData ();
			} else if (button == "rate") {
				#if UNITY_ANDROID
				Application.OpenURL("market://details?id=com.GorillaGames.Stripes");
				#elif UNITY_IPHONE
				Application.OpenURL("itms-apps://itunes.apple.com/app/idYOUR_ID");
				#endif
			}
			audioSource.volume = GameManager.clickVolume;
			audioSource.Play ();
			//StartCoroutine (waitForClick(button));
		}
			
	}

	IEnumerator waitForClick(string sceneName)
	{
		yield return new WaitForSeconds(audioSource.clip.length);
		//gameObject.GetComponent<AudioListener>().enabled = false;
		SceneManager.LoadScene(sceneName);
	}
}
