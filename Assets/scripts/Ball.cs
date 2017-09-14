using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickPoly;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour {

    private Rigidbody2D ball;
    public static bool destroyBall;

    public AudioClip spikes;

	// Use this for initialization
	void Start () {
        ball = gameObject.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        //Vector3 pos = ball.position;
        //pos.x = Mathf.Clamp(pos.x, Walls.leftBarrier, Walls.rightBarrier);
        //pos.y = Mathf.Clamp(pos.y, Walls.bottomBarrier, Walls.topBarrier);
        //ball.position = pos;

        if (destroyBall)
        {
            fadeBall();
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

    void deleteBall()
    {
        Debug.Log("ball deleted");
        BallLauncher.launchedBalls.Remove(gameObject);
        Destroy(gameObject);
		if (BallLauncher.numBarriers > 0) {
			//SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
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

    private void fadeBall()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.GetComponent<Animator>().SetTrigger("destroy");
        destroyBall = false;
    }

    private void restartLevel()
    {
        fadeBall();
        BallLauncher.spikesHit = true;
		AudioSource.PlayClipAtPoint(spikes, Vector3.zero, GameManager.spikesVolume);

		if (GameManager.levelsCompleted < 12)
			GameManager.levelsCompleted += 0.2f;
    }
}
