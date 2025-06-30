using UnityEngine;
using System;

[Serializable]
public class MovementPath
{
    public float MaxDist
    {
        get;
        private set;
    }

    public float[] Distances
    {
        get;
        private set;
    }

    private Vector3[] _nodes;

    public Vector3 this[int i]
    {
        get
        {
            return _nodes[i];
        }

        set
        {
            _nodes[i] = value;
        }
    }

    public int Length
    {
        get
        {
            return _nodes.Length;
        }
    }

    public Vector3 EndNode
    {
        get
        {
            return _nodes[_nodes.Length - 1];
        }
    }

    
    public MovementPath(Vector3[] nodes)
    {
        this._nodes = nodes;
        CalcDistances();
    }
    
    private void CalcDistances()
    {
        Distances = new float[_nodes.Length];
        Distances[0] = 0;

        for (var i = 0; i < _nodes.Length - 1; i++)
        {
            Distances[i + 1] = Distances[i] + Vector3.Distance(_nodes[i], _nodes[i + 1]);
        }

        MaxDist = Distances[Distances.Length - 1];
    }


    public void Draw()
    {
        /*for (int i = 0; i < _nodes.Length - 1; i++)
        {
            Debug.DrawLine(_nodes[i], _nodes[i + 1], Color.cyan, 0.0f, false); //TODO
        }*/
    }
    
    
    public float GetParam(Vector3 position)
    {
        int closestSegment = GetClosestSegment(position);
        float param = this.Distances[closestSegment] + GetParamForSegment(position, _nodes[closestSegment], _nodes[closestSegment + 1]);
        return param;
    }

    public int GetClosestSegment(Vector3 position)
    {
        float closestDist = DistToSegment(position, _nodes[0], _nodes[1]);
        int closestSegment = 0;

        for (int i = 1; i < _nodes.Length - 1; i++)
        {
            float dist = DistToSegment(position, _nodes[i], _nodes[i + 1]);

            if (dist <= closestDist)
            {
                closestDist = dist;
                closestSegment = i;
            }
        }

        return closestSegment;
    }


    public Vector3 GetPosition(float param)
    {
        if (param < 0)
        {
            param = 0;
        }
        else if (param > MaxDist)
        {
            param = MaxDist;
        }

        int i = 0;
        for (; i < Distances.Length; i++)
        {
            if (Distances[i] > param)
            {
                break;
            }
        }

        if (i > Distances.Length - 2)
        {
            i = Distances.Length - 2;
        }
        else
        {
            i -= 1;
        }

        float t = (param - Distances[i]) / Vector3.Distance(_nodes[i], _nodes[i + 1]);
        return Vector3.Lerp(_nodes[i], _nodes[i + 1], t);
    }
    
    float DistToSegment(Vector3 p, Vector3 v, Vector3 w)
    {
        Vector3 vw = w - v;

        float l2 = Vector3.Dot(vw, vw);

        if (l2 == 0)
        {
            return Vector3.Distance(p, v);
        }

        float t = Vector3.Dot(p - v, vw) / l2;

        if (t < 0)
        {
            return Vector3.Distance(p, v);
        }

        if (t > 1)
        {
            return Vector3.Distance(p, w);
        }

        Vector3 closestPoint = Vector3.Lerp(v, w, t);

        return Vector3.Distance(p, closestPoint);
    }
    
    float GetParamForSegment(Vector3 p, Vector3 v, Vector3 w)
    {
        Vector3 vw = w - v;

        vw.y = 0f;

        float l2 = Vector3.Dot(vw, vw);

        if (l2 == 0)
        {
            return 0;
        }

        float t = Vector3.Dot(p - v, vw) / l2;

        if (t < 0)
        {
            t = 0;
        }
        else if (t > 1)
        {
            t = 1;
        }

        return t * (v - w).magnitude;
    }

    public void RemoveNode(int i)
    {
        Vector3[] newNodes = new Vector3[_nodes.Length - 1];

        int newNodesIndex = 0;
        for (int j = 0; j < newNodes.Length; j++)
        {
            if (j != i)
            {
                newNodes[newNodesIndex] = _nodes[j];
                newNodesIndex++;
            }
        }

        this._nodes = newNodes;
        CalcDistances();
    }

    public void ReversePath()
    {
        Array.Reverse(_nodes);

        CalcDistances();
    }
}
