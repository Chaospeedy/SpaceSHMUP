using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy_3 : Enemy
{
    [Header("Enemy_3 Inscribed Fields")]
    public float lifeTime = 5;
    public Vector2 midpointYRange = new Vector2(1.5f, 3);
    [Tooltip("If true, the Bezier points & path are drawn in the scene pane.")]
    public bool drawDebugInfo = true;

    [Header("Enemy_3 private Fields")]
    [SerializeField] private Vector3[] points;
    [SerializeField] private float birthTime;
    // Start is called before the first frame update
    void Start()
    {
        points = new Vector3[3];

        points[0] = pos;

        float xMin = -bndCheck.camWidth + bndCheck.radius;
        float xMax = bndCheck.camWidth - bndCheck.radius;

        points[1] = Vector3.zero;
        points[1].x = Random.Range(xMin,xMax);
        float midYMult = Random.Range(midpointYRange[0], midpointYRange[1]);
        points[1].y = -bndCheck.camHeight * midYMult;

        birthTime = Time.time;

        if(drawDebugInfo)  DrawDebug();
    }

    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;

        if(u > 1){
            Destroy(this.gameObject);
            return;
        }

        //transform.rotation = Quaternion.Euler(u * 180, 0 ,0);

        //This does not work as the book says as "vList" is not a thing.
        //The book does not explain what this is and I can not find anything about it online
        //Therefor I just gave enemy 3 the same movement as enemy 0 except I made it faster.
        //I commented out the rotation above so it doesn't look wierd.
        //The offending code is commented out so that it can compile.

        //u = u - 0.1f *Mathf.Sin(u *Mathf.PI *2);
        //pos = Utils.Bezier(u, points);

        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    void DrawDebug(){
        Debug.DrawLine(points[0], points[1], Color.cyan, lifeTime);
        Debug.DrawLine(points[1], points[2], Color.yellow, lifeTime);

        float numSections = 20;

        Vector3 prevPoint = points[0];
        Color col;
        Vector3 pt;

        for(int i = 1; i < numSections; i++){
            float u = i / numSections;
            pt = Utils.Bezier(u, points);
            col = Color.Lerp(Color.cyan, Color.yellow, u);
            Debug.DrawLine(prevPoint, pt, col, lifeTime);
            prevPoint = pt;
        }

    }
}
