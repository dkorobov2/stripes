using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuickPoly;
using System.Linq;
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
    private float transitionT, animationSpeed;

    public int maxHits;
    public GameObject hits;

    public AudioClip pop;

	public ParticleSystem particles;

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

		timeDelayFade = 0.5f;
		timeDelayReset = 0.5f;
		animationSpeed = 0.3f;
        //audioSource = GetComponent<AudioSource>();

		fade = false;
		reset = false;

		hitsText = hits.GetComponent<TextMesh>();
		hitsText.text = numHitsLeft.ToString();

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
				if (timeDelayReset == animationSpeed) {
					barrierResetColorA = gameObject.GetComponent<QuickPolygon> ().GetFillColorA ();
					barrierResetColorB = Color.Lerp (colors [transitionNum], colors [transitionNum - 1], transitionT);
					barrierResetColorB.a = 1.0f;

					if (barrierResetColorA.a != 1.0f)
						gameObject.transform.parent.transform.parent.GetComponent<Animator>().SetTrigger("reset");
				}

				gameObject.GetComponent<QuickPolygon> ().SetFillUnicolor (Color.Lerp (barrierResetColorB, barrierResetColorA, timeDelayReset/animationSpeed), true);
				timeDelayReset -= Time.deltaTime;
			} else {
				gameObject.GetComponent<QuickPolygon> ().SetFillUnicolor (barrierResetColorB, true);
				reset = false;
			}
		}	

		else if (fade == true) {
			//Debug.Log (gameObject.GetComponent<QuickPolygon> ().GetFillColorA ());
			if (timeDelayFade > 0) {
				if (timeDelayFade == animationSpeed) {
					barrierFadeColorA = gameObject.GetComponent<QuickPolygon> ().GetFillColorA ();
					barrierFadeColorB = barrierFadeColorA;
					barrierFadeColorA.a = 1.0f;
					barrierFadeColorB.a = 0.0f;

					gameObject.transform.parent.transform.parent.GetComponent<Animator>().SetTrigger("fade");
				}
				gameObject.GetComponent<QuickPolygon> ().SetFillUnicolor (Color.Lerp (barrierFadeColorB, barrierFadeColorA, timeDelayFade/animationSpeed), true);

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
			CameraShake.shakeDuration = 0.02f;

            hitsText.text = numHitsLeft.ToString();
            if (numHitsLeft == 0)
            {
				CameraShake.shakeAmount = 0.2f;
				CameraShake.shakeDuration = 0.4f;
				CameraShake.decreaseFactor = 0.9f;
                BallLauncher.numBarriers--;
                fadeBarrier();
                if (BallLauncher.numBarriers == 0)
                {
                    //BallLauncher.paused = true;
					Ball deadBall = BallLauncher.launchedBalls.Last().GetComponent<Ball> ();
					deadBall.fadeBall ();
                }
                return;
            }

			Ball.stripeHit = true;
            transitionNum = 1 + (maxHits - numHits - 1) / hitsInTransition;
            setTransitionT();

			Color collisionColor = Color.Lerp (colors [transitionNum], colors [transitionNum - 1], transitionT);
            gameObject.GetComponent<QuickPolygon>().SetFillUnicolor(collisionColor, true);

			var particleColor = particles.colorOverLifetime;
			particleColor.enabled = true;

			Gradient grad = new Gradient();
			grad.SetKeys( new GradientColorKey[] { new GradientColorKey(collisionColor, 0.0f), new GradientColorKey(colors[transitionNum - 1], 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) } );

			particleColor.color = grad;
			particles.transform.position = col.transform.position;
			particles.Play ();
        }
    }

    private void fadeBarrier()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
		timeDelayFade = animationSpeed;
		timeDelayReset = animationSpeed;
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

		timeDelayFade = animationSpeed;
		timeDelayReset = animationSpeed;
		reset = true;
		fade = false;
	}
}
