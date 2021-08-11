using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    [SerializeField] private DrawConvexHull drawConvexHull;
    [SerializeField] private bool isCollided = false;
    [SerializeField] private Camera cam;
    private void Update()
    {
        if (drawConvexHull.ConvexHull.Count == 0)
        {
            return;
        }

        isCollided = CheckIfInsideConvexHull();
        if(isCollided)
        {
            cam.backgroundColor = new Color32(255, 51, 0, 255);
        }
        else
        {
            cam.backgroundColor = new Color32(0, 255, 0, 255);

        }

    }

    private bool CheckIfInsideConvexHull()
    {
        List<GameObject> convexHull = new List<GameObject>(drawConvexHull.ConvexHull);
        float degree = 0;
        for (int i = 0; i < convexHull.Count; i++)
        {
            Vector3 a = convexHull[i].transform.position;
            //Vector3 b = convexHull[i + 1].transform.position;
            Vector3 b = new Vector3(0, 0, 0);

            ////If we reach the last point of the of the convexHull use the first one as the second part of the triangle 
            if (i == convexHull.Count - 1)
            {
                b = convexHull[0].transform.position;
            }
            else
            {
                b = convexHull[i + 1].transform.position;
            }
            //Calculate distance of vector
            float A = Vector2.Distance(a, b);
            float B = Vector2.Distance(transform.position, a);
            float C = Vector2.Distance(transform.position, b);

            //Calcluate direction of vector
            float ta_x = a.x - transform.position.x;
            float ta_y = a.y - transform.position.y;
            float tb_x = b.x - transform.position.x;
            float tb_y = b.y - transform.position.y;

            float cross = tb_y * ta_x - tb_x * ta_y;

            bool clockwise = (cross < 0) ? true : false;
            //Debug.Log("Degree of each triangle " + (Mathf.Acos((B * B + C * C - A * A) / (2.0f * B * C))) * Mathf.Rad2Deg + " A: " + A + " B: " + B + " C: " + C);
            //bool clockwise = false;
            if (clockwise)
            {
                degree += (Mathf.Acos((B * B + C * C - A * A) / (2.0f * B * C))) * Mathf.Rad2Deg;
            }
            else
            {
                degree -= (Mathf.Acos((B * B + C * C - A * A) / (2.0f * B * C))) * Mathf.Rad2Deg;
            }
            
        }
        Debug.Log(Mathf.Abs(Mathf.Round(degree)) - 360);
        if (Mathf.Abs(Mathf.Round(degree)) - 360 >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
