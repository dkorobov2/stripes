using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour {
    [System.Serializable]
    public struct StripesData
    {
        public int maxLevel;
		public bool sound;
		public float levelsCompleted;
    }

    public static string fileName = "stripesData";
    public static StripesData gameData;

    public static int numLevels = 80;
    public static int maxLevel;
	public static bool sound;

    public static float clickVolume = 0.5f;
	public static float levelVolume = 0.1f;
	public static float spikesVolume = 0.5f;
	public static float popVolume = 1.0f;

	public static float levelsCompleted;
	public static float levelsBeforeAd = 15.0f;

	//public static bool fade;

    public AudioSource audioSource;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
        //gameObject.GetComponent<AudioListener>().enabled = true;
		//fade = true;
        Load();

        if (maxLevel == 0)
        {
            maxLevel = 1;
			sound = true;
			levelsCompleted = 0;

			gameData.maxLevel = maxLevel;
			gameData.sound = sound;
			gameData.levelsCompleted = levelsCompleted;

            Save();
        }

		AudioListener.pause = !sound;
		Advertisement.Initialize ("1512965", false);

        //maxLevel = stripesData.maxLevel;
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			string scene = SceneManager.GetActiveScene ().name;
			if (scene == "start" || scene == "menu")
				Application.Quit();
			else
				SceneManager.LoadScene("menu");
		}
	}

    public static void updateData(int level)
    {
		gameData.levelsCompleted = levelsCompleted;
        if (level > maxLevel) {
			maxLevel = level;
			gameData.maxLevel = maxLevel;
        }
		Save();
    }

	public static void updateSound(bool sound_toggle)
	{
		AudioListener.pause = !sound_toggle;
		sound = sound_toggle;
		gameData.sound = sound_toggle;
		Save ();
	}

	public static void resetData()
	{
		gameData.maxLevel = 1;
		maxLevel = 1;
		Save();
	}

    public static void Save()
    {
        BayatGames.SaveGameFree.SaveGame.Save<StripesData>(fileName, gameData);
    }

    public void Load()
    {
        gameData = BayatGames.SaveGameFree.SaveGame.Load<StripesData>(fileName, new StripesData());
        maxLevel = gameData.maxLevel;
		sound = gameData.sound;
		levelsCompleted = gameData.levelsCompleted;
    }
}
