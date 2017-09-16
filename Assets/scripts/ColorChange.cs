using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickPoly;
using UnityEngine.SceneManagement;

public class ColorChange : MonoBehaviour {
    // color change
    private int numHits;
    private int numTransitions;
    private Color[] colors;
    private int numHitsLeft;
    private TextMesh hitsText;
    private int transitionNum, hitsInTransition;
	private int maxHitsSaved;
    private float transitionT;

    public int maxHits;
    public GameObject hits;

    public AudioClip pop;
    //AudioSource audioSource;
	private float timeDelayFade, timeDelayReset;

	private bool fade, reset;

	private Color barrierFadeColorA, barrierFadeColorB, barrierResetColorA, barrierResetColorB;
	//private Color textColorA, textColorB;
	//private Color circleInnerColorA, circleInnerColorB, circleOuterColorA, circleOuterColorB;

	private GameObject circle;
	private float innerScale, outerScale;

    // Use this for initialization
    void Start () {
        // color change
        numHits = 0;
        numTransitions = 4;
        numHitsLeft = maxHits;
		maxHitsSaved = maxHits;
        hitsInTransition = 5;

		timeDelayFade = 1.0f;
		timeDelayReset = 1.0f;
        //audioSource = GetComponent<AudioSource>();

		fade = false;
		reset = false;

		hitsText = hits.GetComponent<TextMesh>();
		hitsText.text = numHitsLeft.ToString();
		/*
		textColorA = hitsText.color;
		textColorB = textColorA;
		textColorB.a = 0.0f;

		circle = gameObject.transform.parent.FindChild ("circle").gameObject;
		circleInnerColorA = circle.GetComponent<QuickPolygon> ().GetFillColorA ();
		circleInnerColorB = circleInnerColorA;
		circleInnerColorB.a = 0.0f;

		circleOuterColorA = circle.GetComponent<QuickPolygon> ().GetBorderColor ();
		circleOuterColorB = circleOuterColorA;
		circleOuterColorB.a = 0.0f;

		innerScale = circle.GetComponent<QuickPolygon> ().GetBorderInnerScale ();
		outerScale = circle.GetComponent<QuickPolygon> ().GetBorderOuterScale ();
		*/
        colors = new Color[numTransitions + 1];
        //colors[0] = gameObject.GetComponent<QuickPolygon>().GetFillColorA();
        colors[4] = new Color(0.137f, 0.808f, 0.42f);
        colors[3] = new Color(0.255f, 0.459f, 0.569f);
        colors[2] = new Color(0.776f, 0.929f, 0.286f);
        colors[1] = new Color(0.478f, 0.388f, 0.647f);
        colors[0] = new Color(0.6f, 0.133f, 0.4f);

        transitionNum = 1 + (maxHits - numHits - 1) / hitsInTransition;
        setTransitionT();
        gameObject.GetComponent<QuickPolygon>().SetFillUnicolor(Color.Lerp(colors[transitionNum], colors[transitionNum - 1], transitionT), true);

        gameObject.transform.parent.transform.parent.GetComponent<Animator>().enabled = true;
    }

	void Update()
	{
		//if (reset == true && fade == false && BallLauncher.numBarriers > 0) {
		if (reset == true && BallLauncher.numBarriers > 0) {
			if (timeDelayReset > 0) {
				if (timeDelayReset == 1.0f) {
					barrierResetColorA = Color.Lerp (colors [transitionNum], colors [transitionNum - 1], transitionT);
					barrierResetColorB = barrierResetColorA;
					barrierResetColorA.a = 0.0f;
					barrierResetColorB.a = 1.0f;

					gameObject.transform.parent.transform.parent.GetComponent<Animator>().SetTrigger("reset");
				}

				gameObject.GetComponent<QuickPolygon> ().SetFillUnicolor (Color.Lerp (barrierResetColorB, barrierResetColorA, timeDelayReset), true);
				//circle.GetComponent<QuickPolygon> ().SetFillUnicolor (Color.Lerp (circleInnerColorA, circleInnerColorB, timeDelayReset), true);
				//circle.GetComponent<QuickPolygon> ().SetBorderUnicolor (Color.Lerp (circleOuterColorA, circleOuterColorB, timeDelayReset), outerScale, innerScale, true);
				//hitsText.color = Color.Lerp (textColorA, textColorB, timeDelayReset);

				timeDelayReset -= Time.deltaTime;
			} else {
				gameObject.GetComponent<QuickPolygon> ().SetFillUnicolor (barrierResetColorB, true);
				reset = false;
			}
		}	

		else if (fade == true) {
			//Debug.Log (gameObject.GetComponent<QuickPolygon> ().GetFillColorA ());
			if (timeDelayFade > 0) {
				if (timeDelayFade == 1.0f) {
					barrierFadeColorA = gameObject.GetComponent<QuickPolygon> ().GetFillColorA ();
					barrierFadeColorB = barrierFadeColorA;
					barrierFadeColorA.a = 1.0f;
					barrierFadeColorB.a = 0.0f;

					gameObject.transform.parent.transform.parent.GetComponent<Animator>().SetTrigger("fade");
				}
				gameObject.GetComponent<QuickPolygon> ().SetFillUnicolor (Color.Lerp (barrierFadeColorB, barrierFadeColorA, timeDelayFade), true);
				//circle.GetComponent<QuickPolygon> ().SetFillUnicolor (Color.Lerp (circleInnerColorB, circleInnerColorA, timeDelayFade), true);
				//circle.GetComponent<QuickPolygon> ().SetBorderUnicolor (Color.Lerp (circleOuterColorB, circleOuterColorA, timeDelayFade), outerScale, innerScale, true);

				/*
				if (circle.GetComponent<MeshRenderer> ().material.HasProperty ("_ReplaceColor")) {
					Debug.Log ("********************************");
				} else {
					Debug.Log ("NOOOOO");
				}
				circle.GetComponent<MeshRenderer>().material.SetColor("_ReplaceColor", Color.red);
				*/

				//hitsText.color = Color.Lerp (textColorB, textColorA, timeDelayFade);

				timeDelayFade -= Time.deltaTime;
			} else {
				gameObject.GetComponent<QuickPolygon> ().SetFillUnicolor (barrierFadeColorB, true);
				fade = false;
			}
		}
	}

    void OnCollisionEnter2D(Collision2D col)
    {
		if (gameObject.tag == "barrier" && !BallLauncher.launching)
        {
            numHits++;
            numHitsLeft--;

            //audioSource.PlayOneShot(pop);
			AudioSource.PlayClipAtPoint(pop, Vector3.zero, GameManager.popVolume);

            hitsText.text = numHitsLeft.ToString();
            if (numHitsLeft == 0)
            {
                BallLauncher.numBarriers--;
                fadeBarrier();
                if (BallLauncher.numBarriers == 0)
                {
                    //BallLauncher.paused = true;
                    Ball.destroyBall = true;
                }
                return;
            }

            transitionNum = 1 + (maxHits - numHits - 1) / hitsInTransition;
            setTransitionT();
            gameObject.GetComponent<QuickPolygon>().SetFillUnicolor(Color.Lerp(colors[transitionNum], colors[transitionNum - 1], transitionT), true);
        }
    }
    /*
    IEnumerator resetLevel()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    */

    private void fadeBarrier()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
		timeDelayFade = 1.0f;
		timeDelayReset = 1.0f;
		fade = true;
		reset = false;
    }

    private void setTransitionT()
    {
        transitionT = (float)(hitsInTransition - ((numHitsLeft - 1) % hitsInTransition) - 1) / (float)hitsInTransition;
        //Debug.Log(transitionNum + " " + transitionT);
    }

	public void resetBarrier()
	{
		int prevNumHitsLeft = numHitsLeft;

		numHits = 0;
		maxHits = maxHitsSaved;
		numHitsLeft = maxHitsSaved;
		hitsText.text = numHitsLeft.ToString();

		transitionNum = 1 + (maxHits - numHits - 1) / hitsInTransition;
		setTransitionT();

		gameObject.GetComponent<Collider2D>().enabled = true;

		//if (gameObject.GetComponent<QuickPolygon> ().GetFillColorA ().a < 1.0f) {
		if (prevNumHitsLeft == 0) {
			timeDelayFade = 1.0f;
			timeDelayReset = 1.0f;
			reset = true;
			fade = false;
		} else {
			//Debug.Log(gameObject.GetComponent<QuickPolygon> ().GetFillColorA ().a);
			gameObject.GetComponent<QuickPolygon>().SetFillUnicolor(Color.Lerp(colors[transitionNum], colors[transitionNum - 1], transitionT), true);
		}
	}
}
