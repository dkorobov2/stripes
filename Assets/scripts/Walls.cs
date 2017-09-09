using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour {

    public PhysicsMaterial2D bounceMaterial;

    static public float leftBarrier;
    static public float rightBarrier;
    static public float topBarrier;
    static public float bottomBarrier;

    private GameObject left;
    private GameObject right;
    private GameObject top;
    private GameObject bottom;

    private BoxCollider2D leftWall;
    private BoxCollider2D rightWall;
    private BoxCollider2D topWall;
    private BoxCollider2D bottomWall;

    // Use this for initialization
    void Awake()
    {
        float colliderWidth = 0.1f;

        left = new GameObject();
        left.name = "left";

        right = new GameObject();
        right.name = "right";

        top = new GameObject();
        top.name = "top";

        bottom = new GameObject();
        bottom.name = "bottom";

        GameObject score = transform.FindChild("score").gameObject;

        Vector2 topRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        Vector2 bottomRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 0));
        Vector2 topLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 1));
        Vector2 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        leftBarrier = topLeft.x + colliderWidth;
        rightBarrier = bottomRight.x - colliderWidth;
        topBarrier = topRight.y - colliderWidth;
        //topBarrier = score.transform.position.y - score.transform.localScale.y/2;
        //Debug.Log("top barrier: " + topBarrier);
        bottomBarrier = bottomRight.y + colliderWidth;

        // bottom wall
        bottom.transform.parent = this.transform;
        bottomWall = bottom.AddComponent<BoxCollider2D>();
        bottomWall.transform.position = (bottomLeft+bottomRight)/2;
        bottomWall.transform.localScale = new Vector3(bottomRight.x - bottomLeft.x, colliderWidth, 1);
        bottomWall.sharedMaterial = bounceMaterial;

        // top wall
        top.transform.parent = this.transform;
        topWall = top.AddComponent<BoxCollider2D>();
        topWall.transform.position = (topLeft + topRight) / 2;
        topWall.transform.position = new Vector3(topWall.transform.position.x, topBarrier, topWall.transform.position.z);
        //topWall.transform.position = new Vector3(topWall.transform.position.x, topBarrier, topWall.transform.position.z);
        topWall.transform.localScale = new Vector3(topRight.x - topLeft.x, colliderWidth, 1);
        topWall.sharedMaterial = bounceMaterial;

        // left wall
        left.transform.parent = this.transform;
        leftWall = left.AddComponent<BoxCollider2D>();
        leftWall.transform.position = (topLeft + bottomLeft) / 2;
        leftWall.transform.localScale = new Vector3(colliderWidth, bottomLeft.y - topLeft.y, 1);
        leftWall.sharedMaterial = bounceMaterial;

        // right wall
        right.transform.parent = this.transform;
        rightWall = right.AddComponent<BoxCollider2D>();
        rightWall.transform.position = (bottomRight + topRight) / 2;
        rightWall.transform.localScale = new Vector3(colliderWidth, bottomRight.y - topRight.y, 1);
        rightWall.sharedMaterial = bounceMaterial;
    }
}
