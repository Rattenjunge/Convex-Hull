using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pointPref;
    [SerializeField] private Camera gameCamera;
    [SerializeField] private Vector2[] spawnArea = new Vector2[2];
    [SerializeField, Range(3,400)] private int spawnAmount = 8;
    [SerializeField] int maxIterations = 30;
    public List<GameObject> pointList = new List<GameObject>();
    [SerializeField,Range(0.1f,1f)] private float minDistance = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        CalculateSpawnArea();
        for (int i = 0; i < spawnAmount; i++)
        {
            pointList.Add(Instantiate(pointPref, new Vector3(0, 0, 0), Quaternion.identity));
            pointList = PointRandomizer.RandomizePosition(pointList, spawnArea, minDistance, maxIterations);
        }
    }

    void CalculateSpawnArea()
    {
        Vector2 topLeft = gameCamera.ScreenToWorldPoint(new Vector3(0, gameCamera.pixelHeight, 0));
        Vector2 bottomRight = gameCamera.ScreenToWorldPoint(new Vector3(gameCamera.pixelWidth, 0, 0));
        spawnArea[0] = topLeft;
        spawnArea[1] = bottomRight;

        //Make SpawnArea smaller for better spawn results (borderclipping)
        //top left x&y (so they are closer to the center of the frame)
        spawnArea[0][0] += 2;
        spawnArea[0][1] -= 2;
        //bottom right x&y -2 (so they are closer to the center of the frame)
        spawnArea[1][0] -= 2;
        spawnArea[1][1] += 2;

        Debug.Log(spawnArea[0] + ":" + spawnArea[1]);
    }
}
