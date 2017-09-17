using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class BallLauncher : MonoBehaviour
{
	private bool mousePressed;
	private float arrowDistance, fingerDistance;
	private Vector2 pivot;
	private Vector2 pausedVelocity;
	private int ballsLaunched;

	public GameObject arrow;
	public GameObject ball;
	public int ballForce;

	public static List<GameObject> launchedBalls;
	public static bool paused, unpaused, freeze;
	public static bool spikesHit;
	public static int numBarriers;
	public static int level;
	public static bool launching;

	void Awake()
	{
		Physics2D.IgnoreLayerCollision(8, 9);
	}
	// Use this for initialization
	void Start()
	{
		mousePressed = false;
		paused = false;
		unpaused = false;
		freeze = false;
		spikesHit = false;
		launching = false;
		ballsLaunched = 0;
		arrowDistance = 0.5f;
		fingerDistance = 0;
		initNumBarriers();

		launchedBalls = new List<GameObject>();

		level = System.Convert.ToInt32(SceneManager.GetActiveScene().name);

		GameManager.levelsCompleted++;
		GameManager.updateData (level);

		// show ad
		if (GameManager.levelsCompleted > GameManager.levelsBeforeAd) {
			Advertisement.Show ();
			GameManager.levelsCompleted -= GameManager.levelsBeforeAd;
			Debug.Log ("ad showing");
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (spikesHit)
		{
			spikesHit = false;
			ballsLaunched--;
			launching = false;
			mousePressed = false;
			arrow.SetActive(false);
			initNumBarriers();
		}

		if (paused)
		{
			Debug.Log("paused");
			//Time.timeScale = 0;
			paused = false;
			freeze = true;
			launching = false;

			if (launchedBalls.Count != 0)
			{
				pausedVelocity = launchedBalls[launchedBalls.Count - 1].GetComponent<Rigidbody2D>().velocity;
				launchedBalls[launchedBalls.Count - 1].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			}

		}
		else if (unpaused)
		{
			Debug.Log("unpaused");
			unpaused = false;
			freeze = false;
			launching = false;
			//Time.timeScale = 1;

			if (launchedBalls.Count != 0)
			{
				launchedBalls[launchedBalls.Count - 1].GetComponent<Rigidbody2D>().velocity = pausedVelocity;
			}

		}

		if (Input.GetMouseButtonDown(0) && !mousePressed && !freeze)
		{
			RaycastHit2D hit;
			try
			{
				hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			}
			catch (System.NullReferenceException)
			{
				return;
			}

            //if (hit.transform == null && !EventSystem.current.IsPointerOverGameObject() && ballsLaunched == 0)
            if (hit.transform == null && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && ballsLaunched == 0)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (pos.x > Walls.leftBarrier + 0.15f && pos.x < Walls.rightBarrier - 0.15f && pos.y < Walls.topBarrier - 0.15f && pos.y > Walls.bottomBarrier + 0.15f)
                {

                    Debug.Log("launching");

                    if (level == 1)
                        GameObject.Find("tutorial").GetComponent<Animator>().enabled = true;

                    mousePressed = true;
                    launchedBalls.Add(Instantiate(ball, gameObject.transform));
                    launching = true;

                    ballsLaunched++;
                    pivot = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    arrow.transform.position = pivot + new Vector2(0, arrowDistance);
                    //arrow.transform.position = pivot + new Vector2(0, 0);
                    arrow.transform.eulerAngles = new Vector3(0, 0, 0);
                    launchedBalls[launchedBalls.Count - 1].transform.position = new Vector3(pivot.x, pivot.y, 5);

                    launchedBalls[launchedBalls.Count - 1].GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                    arrow.SetActive(true);
                    launchedBalls[launchedBalls.Count - 1].SetActive(true);
                }
			}
		}

		if (mousePressed && ballsLaunched > 0 && launching)
		{
			//Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			float rotation;
			Vector2 currPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 offset = pivot - currPos;
			fingerDistance = Vector2.Distance(pivot, currPos);
			/*
            fingerDistance = Vector2.Distance(pivot, currPos);

            if (fingerDistance > arrowDistance)
                fingerDistance = arrowDistance;

            offset = offset.normalized * fingerDistance;
            */
			offset = offset.normalized * arrowDistance;
			if (offset == Vector2.zero)
			{
				offset.x = 0;
				offset.y = arrowDistance;
				rotation = 0;
			}
			else
			{
				currPos = currPos - pivot;
				rotation = (180 * Mathf.Atan2(currPos.y, currPos.x) / Mathf.PI) + 90;
			}

			//Debug.Log(rotation);
			arrow.transform.position = pivot + (Vector2) offset;

			arrow.transform.eulerAngles = new Vector3(0, 0, rotation);
			/*
            Color color = arrow.GetComponent<SpriteRenderer>().material.color;
            color.a = 255 * (fingerDistance / arrowDistance);
            Debug.Log(color.a);
            arrow.GetComponent<SpriteRenderer>().material.color = color;
            */
		}

		if (Input.GetMouseButtonUp(0) && mousePressed)
		{
			if (ballsLaunched > 0 && launching) { 
				mousePressed = false;
				launching = false;

				arrow.SetActive(false);
				Vector2 ballDirection = pivot - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
				if (fingerDistance < arrowDistance/2)
				{
					GameObject tempBall = launchedBalls[launchedBalls.Count - 1];
					launchedBalls.RemoveAt(launchedBalls.Count - 1);
					Destroy(tempBall);
					ballsLaunched--;
				}
				else
				{
					launchedBalls[launchedBalls.Count - 1].GetComponent<Rigidbody2D>().AddForce(ballDirection.normalized * ballForce);
				}
			}
		}
	}

	private void initNumBarriers()
	{
		numBarriers = GameObject.Find("Stripes").transform.childCount;
		//Debug.Log(numBarriers);
	}
	/*
	private void RequestBanner()
	{
		#if UNITY_EDITOR
		string adUnitId = "unused";
		#elif UNITY_ANDROID
		string adUnitId = "INSERT_ANDROID_BANNER_AD_UNIT_ID_HERE";
		#elif UNITY_IPHONE
		string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
		#else
		string adUnitId = "unexpected_platform";
		#endif

		// Create a 320x50 banner at the top of the screen.
		BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
		// Create an empty ad request.
		//AdRequest request = new AdRequest.Builder().Build();

		AdRequest request = new AdRequest.Builder()
		.AddTestDevice(AdRequest.TestDeviceSimulator)       // Simulator.
		.AddTestDevice("41734EB9023BF7C87E15358E81921774")  // My test device.
		.Build();

		// Load the banner with the request.
		bannerView.LoadAd(request);
	}
	*/
}