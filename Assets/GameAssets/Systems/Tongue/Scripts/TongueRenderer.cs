using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class TongueRenderer : MonoBehaviour
{
    public Spline spline;
    public Transform tongueFollowPoint;
    private SplineContainer _splineContainer;


    void Start()
    {
        _splineContainer = GetComponent<SplineContainer>();
        spline =_splineContainer.Spline;
        
        AddTonguePoint(tongueFollowPoint.localPosition);
    }


    public void AddTonguePoint(Vector3 pos) 
    {
        BezierKnot knot = new BezierKnot(pos);
        knot.TangentIn = new float3(0,1,-1f);
        knot.TangentOut = new float3(0,1,1f);
    
        spline.Add(knot);

        if (GameManager.Instance.SmoothTongueTurns)
        {
            spline.SetTangentMode(TangentMode.AutoSmooth);
        }
        else 
        {
            spline.SetTangentMode(TangentMode.Linear);
        }
    }
    public Vector3[] BezierKnotToVector3(BezierKnot[] knots)
    {
        Vector3[] path = new Vector3[knots.Length];
        for (int i = 0; i < knots.Length; i++)
        {
            path[i] = new Vector3(knots[i].Position.x,knots[i].Position.y,knots[i].Position.z);
        }
        return path;
    }
    public Vector3[] GetTonguePath()
    {
        BezierKnot[] path = new BezierKnot[spline.Count];
        path = spline.ToArray();
        
        Vector3[] pathVector = new Vector3[spline.Count];

        for (int i = 0; i < spline.Count; i++)
        {
            pathVector[i] = path[i].Position;
        }
        return pathVector;
    }

    public float CalculateTotalDistance(Vector3[] path)
    {
        float totalDistance = 0;
        for (int i = 0; i < path.Length - 1; i++)
        {
           totalDistance += Vector3.Distance(path[i], path[i+1]);
        }
        return totalDistance;
    }
}
