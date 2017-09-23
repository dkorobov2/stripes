using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Advertisements;

public class UIControl : MonoBehaviour, IPointerDownHandler {
    public GameObject pauseScreen;

    public GameObject levelCleared;
    public GameObject pause;
    public GameObject obstacles;

    // UI elements
    public Text levelText;
    //public Text bestText;

    public AudioClip clickAudio;
    public AudioClip levelClearedAudio;
    AudioSource audioSource;

    // Use this for initialization
    void Start () {
        levelText.text = SceneManager.GetActiveScene().name;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (BallLauncher.numBarriers == 0)
        {
            levelComplete();
        }
    }
		
	//Do this when the mouse is clicked over the selectable object this script is attached to.
	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log(this.gameObject.name + " Was Clicked.");
	}

    public void pauseGame()
    {
        if (BallLauncher.numBarriers == -1)
            return;
        audioSource.volume = GameManager.clickVolume;
        audioSource.PlayOneShot(clickAudio);
        BallLauncher.paused = true;
        pauseScreen.SetActive(true);
    }

    public void unpauseGame()
    {
        audioSource.volume = GameManager.clickVolume;
        audioSource.PlayOneShot(clickAudio);
        BallLauncher.unpaused = true;
        pauseScreen.SetActive(false);
    }

    public void resetLevel()
    {
        //audioSource.volume = GameManager.clickVolume;
        audioSource.PlayOneShot(clickAudio);
		Advertisement.Show ();
		StartCoroutine(waitForClick((System.Convert.ToInt32(SceneManager.GetActiveScene().name) + 1).ToString()));
    }

    public void loadNextLevel()
    {
        audioSource.volume = GameManager.clickVolume;
        audioSource.PlayOneShot(clickAudio);
        StartCoroutine(waitForClick((System.Convert.ToInt32(SceneManager.GetActiveScene().name) + 1).ToString()));
    }

    public void mainMenu()
    {
        audioSource.volume = GameManager.clickVolume;
        audioSource.PlayOneShot(clickAudio);
        StartCoroutine(waitForClick("menu"));
        
    }

    private void levelComplete()
    {
        Debug.Log("level complete");
        BallLauncher.numBarriers--;
		if (BallLauncher.level == GameManager.numLevels) {
			levelCleared.GetComponent<Text> ().text = "thanks for\n" + "playing!";
			//GameObject.Find ("rate").SetActive (true);
		}

		pause.SetActive(false);
        levelCleared.SetActive(true);
        obstacles.SetActive(false);
        //AudioSource.PlayClipAtPoint(levelClearedAudio, Vector3.zero);
        if (!audioSource.isPlaying && audioSource.clip.loadState == AudioDataLoadState.Loaded)
        {
			audioSource.volume = GameManager.levelVolume;
            audioSource.PlayOneShot(levelClearedAudio);
        }
        //levelCleared.transform.parent.GetComponent<Animator>().enabled = true;
        StartCoroutine(levelCompleteControls());

    }

    IEnumerator levelCompleteControls()
    {
        yield return new WaitForSeconds(levelClearedAudio.length);
        //nextLevel.SetActive(true);
        //restart.SetActive(true);
        //sceneTransition.SetActive(true);
		if (BallLauncher.level == GameManager.numLevels) {
			SceneManager.LoadScene ("menu");
		} else {
			SceneManager.LoadScene((System.Convert.ToInt32(SceneManager.GetActiveScene().name) + 1).ToString());
		}
    }

    IEnumerator waitForClick(string sceneName)
    {
        yield return new WaitForSeconds(clickAudio.length);
        SceneManager.LoadScene(sceneName);
    }
}
