using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LevelSelectClick : MonoBehaviour {

    private AudioSource audioSource;

    void Start()
    {
        //gameObject.GetComponent<EventTrigger>().OnPointerDown(onClick);
        audioSource = GameObject.Find("audio").GetComponent<AudioSource>();
    }

    public void loadSceneClick()
    {
		if (System.Convert.ToInt32(gameObject.name) > GameManager.maxLevel)
			return;
		
        audioSource.volume = GameManager.clickVolume;
        audioSource.Play();
		StartCoroutine(waitForClick(gameObject.name));
    }

	public void loadMenu()
	{
		audioSource.volume = GameManager.clickVolume;
		audioSource.Play();
		StartCoroutine(waitForClick("menu"));
	}

    IEnumerator waitForClick(string sceneName)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        SceneManager.LoadScene(sceneName);
    }
}
