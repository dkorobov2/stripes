using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelSelect : MonoBehaviour {

    public GameObject levelPrefab;
    public GameObject levelsParent;

    public int numLevelsX;

    public FontStyle bold;

    private float leftBarrier;
    private float rightBarrier;
    private float topBarrier;
    private float bottomBarrier;

    // temp variables
    private int columnNum;
    private float offsetX, offsetY;

    // Use this for initialization
    void Start () {
        Vector2 topRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        Vector2 bottomRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 0));
        Vector2 topLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 1));
        Vector2 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        leftBarrier = topLeft.x;
        rightBarrier = bottomRight.x;
        topBarrier = topRight.y;
        bottomBarrier = bottomRight.y;

        offsetX = (rightBarrier - leftBarrier) / (numLevelsX + 1);
        
        for (int i = 1; i <= GameManager.numLevels; i++)
        {
            GameObject button = Instantiate(levelPrefab);
            //button.transform.parent = gameObject.transform;
            button.transform.SetParent(levelsParent.transform, false);
            button.name = i.ToString();
            button.GetComponent<Text>().text = i.ToString();

            if (i <= GameManager.maxLevel)
            {
                button.GetComponent<Text>().color = Color.white;
                if (i == GameManager.maxLevel)
                {
                    button.GetComponent<Text>().fontSize += 2;
                    button.GetComponent<Text>().fontStyle = bold;
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	}
}
