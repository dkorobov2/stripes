using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StripesUtils : MonoBehaviour {
    private AudioSource audioSource;

	static public float leftBarrier;
	static public float rightBarrier;
	static public float topBarrier;
	static public float bottomBarrier;

	void Awake()
	{

		Vector2 topRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
		Vector2 bottomRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 0));
		Vector2 topLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 1));
		Vector2 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

		leftBarrier = topLeft.x;
		rightBarrier = bottomRight.x;
		topBarrier = topRight.y;
		bottomBarrier = bottomRight.y;
	}

    // Use this for initialization
    void Start () {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void loadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void loadSceneClick(string SceneName)
    {
        audioSource.volume = GameManager.clickVolume;
        audioSource.Play();
        StartCoroutine(waitForClick(SceneName));
    }

    public void onPlay()
    {
        audioSource.volume = GameManager.clickVolume;
        audioSource.Play();
        //maxLevel = stripesData.maxLevel;
        StartCoroutine(waitForClick(GameManager.maxLevel.ToString()));
    }

    IEnumerator waitForClick(string sceneName)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        //gameObject.GetComponent<AudioListener>().enabled = false;
        SceneManager.LoadScene(sceneName);
    }
}
