using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickPoly;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour {

    private Rigidbody2D ball;
	private bool revertScale;
	public static bool stripeHit;

    public AudioClip spikes;
    public AudioSource audioSource;

	// Use this for initialization
	void Start () {
		stripeHit = false;
		revertScale = false;
        ball = gameObject.GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        Debug.Log(GameManager.spikesVolume);
        audioSource.volume = GameManager.spikesVolume;
    }

	// Update is called once per frame
	void Update () {
		if (stripeHit) {
			gameObject.transform.localScale = new Vector3 (1.0f, 1.0f, 1f); 
			stripeHit = false;
			revertScale = true;
		} else if (revertScale) {
			gameObject.transform.localScale = new Vector3 (0.7f, 0.7f, 1f); 
			revertScale = false;
		}
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "spike")
        {
            Debug.Log("spikes hit!");
            restartLevel();
        }
    }

    public void deleteBall()
    {
        Debug.Log("ball deleted");
        BallLauncher.launchedBalls.Remove(gameObject);
        Destroy(gameObject);
		if (BallLauncher.numBarriers > 0) {
			resetBarriers ();
		}
    }

    public void fadeBall()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.GetComponent<Animator>().SetTrigger("destroy");
    }

    private void restartLevel()
    {
        fadeBall();
        BallLauncher.spikesHit = true;
		if (!audioSource.isPlaying)
			audioSource.PlayOneShot(spikes);

		if (GameManager.levelsCompleted < GameManager.levelsBeforeAd)
			GameManager.levelsCompleted += 0.1f;
    }

	public void resetBarriers()
	{
		GameObject stripes = GameObject.Find("Stripes");
		GameObject stripe, barrier;
		//foreach (GameObject stripe in stripes) {
		for (int i = 0; i < stripes.transform.childCount; i++) {
			stripe = stripes.transform.GetChild (i).gameObject;
			barrier = stripe.transform.Find ("barrier/barrier").gameObject;
			ColorChange colorChange = barrier.GetComponent<ColorChange> ();
			colorChange.resetBarrier ();
		}
	}
}
