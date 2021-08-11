using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawConvexHull : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PointSpawner pointSpawner;
    [SerializeField] List<GameObject> ConvexHull = new List<GameObject>();
    [SerializeField] private LineRenderer lineRend;


   private void Update()
   {
       if (pointSpawner.pointList.Count > 0)
       {
           ConvexHull = JarvisMarchAlgorithm.GetConvexHull(pointSpawner.pointList);
           if (ConvexHull.Count > 0)
           {
  
               lineRend.positionCount = ConvexHull.Count;
               for (int i = 0; i < ConvexHull.Count; i++)
               {
                   lineRend.SetPosition(i, ConvexHull[i].transform.position);
               }
           }
       }
   }
  

 
}
