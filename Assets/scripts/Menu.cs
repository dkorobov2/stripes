using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	AudioSource audioSource;

    // Use this for initialization
    void Start () {
		audioSource = gameObject.GetComponent<AudioSource>();
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

			if (hit.transform.name == null)
				return;

			Debug.Log (hit.transform.name);
			//hit.transform.gameObject.transform.parent.GetComponent<Animator>().enabled = true;

			string scene = hit.transform.name;

			if (scene != "level select" && scene != "settings" && scene != "play")
				return;
			
			if (scene == "play")
				scene = GameManager.maxLevel.ToString();

			audioSource.volume = GameManager.clickVolume;
			audioSource.Play ();
			StartCoroutine (waitForClick(scene));
		}
	}

    string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    string RandomMenuColor()
    {
        string s = "<color=#" + ColorToHex(new Color(0.137f, 0.808f, 0.42f)) + ">s</color>";
        string t = "<color=#" + ColorToHex(new Color(0.255f, 0.459f, 0.569f)) + ">t</color>";
        string r = "<color=#" + ColorToHex(new Color(0.776f, 0.929f, 0.286f)) + ">r</color>";
        string i = "<color=#" + ColorToHex(new Color(0.478f, 0.388f, 0.647f)) + ">i</color>";
        string p = "<color=#" + ColorToHex(new Color(0.776f, 0.929f, 0.286f)) + ">p</color>";
        string e = "<color=#" + ColorToHex(new Color(0.255f, 0.459f, 0.569f)) + ">e</color>";

        return s+t+r+i+p+e+s;
    }

	IEnumerator waitForClick(string sceneName)
	{
		yield return new WaitForSeconds(audioSource.clip.length);
		//gameObject.GetComponent<AudioListener>().enabled = false;
		SceneManager.LoadScene(sceneName);
	}
}
