using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PointRandomizer
{
    public static List<GameObject> RandomizePosition(List<GameObject> pointList, Vector2[] spawnArea, float minDistance, int maxIterations)
    {
        List<Vector2> forbiddenSpawnLocations = new List<Vector2>();
        for (int i = 0; i < pointList.Count; i++)
        {

            float x = Random.Range(spawnArea[0][0], spawnArea[1][0]);
            float y = Random.Range(spawnArea[0][1], spawnArea[1][1]);
            bool spawnAccepted = false;

            //WARNING: high numbers of points and big distances between points end up in very long calculation time. Keep that in mind.
            // n provides overview how often "while" has to run trhough
            int n = 0;
            while (!spawnAccepted)
            {
                if (n >= maxIterations)
                {
                    spawnAccepted = true;
                    break;
                }
                if (forbiddenSpawnLocations.Count == 0)
                {
                    pointList[i].transform.position = new Vector2(x, y);
                    forbiddenSpawnLocations.Add(new Vector2(x, y));
                    break;
                }


                foreach (var forbiddenSpawn in forbiddenSpawnLocations)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), forbiddenSpawn);
                    if (distance < minDistance)
                    {
                        spawnAccepted = false;
                        break;
                    }
                    else
                    {
                        spawnAccepted = true;
                    }

                }

                if (!spawnAccepted)
                {
                    x = Random.Range(spawnArea[0][0], spawnArea[1][0]);
                    y = Random.Range(spawnArea[0][1], spawnArea[1][1]);
                    n++;
                }
                else
                {
                    pointList[i].transform.position = new Vector2(x, y);
                    forbiddenSpawnLocations.Add(new Vector2(x, y));
                    break;
                }
            }

        }
        return pointList;

    }
}
