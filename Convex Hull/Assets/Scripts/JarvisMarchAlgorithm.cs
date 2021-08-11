
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class JarvisMarchAlgorithm
{

    public static List<GameObject> GetConvexHull(List<GameObject> _points)
    {
        List<GameObject> points = new List<GameObject>(_points);
        
        //if we have just 3 points, we have our convex hull
        if (points.Count == 3)
        {
            //they could be all in a line
            return points;
        }
        //if fewer than 3, there is no way to draw a convex hull
        if (points.Count < 3)
        {
            return null;
        }


        //List for the points which build the convex hull
        List<GameObject> convexHull = new List<GameObject>();

        //Find the furthest left point (smallest X)
        //If there are points with the same smallest x, find the one with the smallest z
        GameObject startPoint = points[0];
        Vector3 startPos = startPoint.transform.position;

        for (int i = 0; i < points.Count; i++)
        {
            Vector3 testPos = points[i].transform.position;

            //Blogpost says because of precision issues, we use Mathf.Approximately to test if the x position are the same
            if (testPos.x < startPos.x || (Mathf.Approximately(testPos.x, startPos.x) && testPos.z < startPos.z))
            {
                startPoint = points[i];
                startPos = startPoint.transform.position;
            }

        }
        //The startpoint is always a part of the convex hull
        convexHull.Add(startPoint);
        points.Remove(startPoint);


        //Loop to generate the convex hull
        GameObject currentPoint = convexHull[0];

        //ColinearPoints storage (blogpost says storring it here is better than creating a list each loop (obviously)
        List<GameObject> colinearPoints = new List<GameObject>();

        int counter = 0;
        while (true)
        {
            //Adding the startposition again after two iterations wwill terminate the alorithm down the line
            //Probably adding the startpoint later so the algorythm cannot find it at the beginning
            if (counter == 2)
            {
               points.Add(convexHull[0]);
            }

            //Pick next point randomly
            GameObject nextPoint = points[Random.Range(0, points.Count)];
            //Convert to 2d space
            Vector2 a = new Vector2(currentPoint.transform.position.x, currentPoint.transform.position.y);
            Vector2 b = new Vector2(nextPoint.transform.position.x, nextPoint.transform.position.y);

            //Check if there is a point to the right of ab, if so, the point will be teh new b 
            for (int i = 0; i < points.Count; i++)
            {
                //Skip the point we picked randomly
                if (points[i].Equals(nextPoint))
                {
                    continue;
                }
                Vector2 c = new Vector2(points[i].transform.position.x, points[i].transform.position.y);


                //Where is c in relation to a-b
                // < 0 -> to the right
                // = 0 -> on the line
                // > 0 -> to the left

                float relation = (a.x - c.x) * (b.y - c.y) - (a.y - c.y) * (b.x - c.x);

                //Colinear points
                //Blogpost says floating points have precision issues (known fact), so we cannot use 0 instead we use a accuracy value
                float accuracy = 0.00001f;

                if (relation < accuracy && relation > -accuracy)
                {
                    colinearPoints.Add(points[i]);
                }

                //To the right = better point. Pick it for the next point on the convex hull
                else if (relation < 0f)
                {
                    nextPoint = points[i];
                    b = new Vector2(nextPoint.transform.position.x, nextPoint.transform.position.y);

                    //Clear colinear points, because we changed direction
                    colinearPoints.Clear();
                }
                //we ignore lefties

            }


            //If we have colinear points
            if (colinearPoints.Count > 0)
            {
                colinearPoints.Add(nextPoint);
                //Sort this list, so we can add the colinear points in correct order
                colinearPoints = colinearPoints.OrderBy(n => Vector3.SqrMagnitude(n.transform.position - currentPoint.transform.position)).ToList();

                convexHull.AddRange(colinearPoints);

                //Last point of the colinearPoints will be our currentPoint where we check other points
                currentPoint = colinearPoints[colinearPoints.Count - 1];

                //Remove the points that are now on the convex hull
               for(int i = 0; i < colinearPoints.Count -1; i++)
                {
                    points.Remove(colinearPoints[i]);
                }
                colinearPoints.Clear();

            }
            //If we haven't found any colinear points, we take the furthest right point and add it to the convexhull. Now nextPoint will be our currentPoint
            else
            {
                convexHull.Add(nextPoint);
                points.Remove(nextPoint);
                currentPoint = nextPoint;
            }

            //Have we found the first point on the hull? if so we have completed the hull
            if(currentPoint.Equals(convexHull[0]))
            {
                //Remove the last one, so there will be no duplicate in the convex hull
                convexHull.RemoveAt(convexHull.Count - 1);
                break;
            }

            counter++;
        }
        return convexHull;
    }

}
