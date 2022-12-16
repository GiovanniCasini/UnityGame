using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleFormation
{
    public Vector2[] CalculateCircleFormation(List<GameObject> entities, Vector3 startingPos, float radius)
    {
        Vector2[] targetPos = new Vector2[entities.Count];
        int count = 0;
        float degree = 360f;
        float direction = -1;
        float arclength = (degree / 360) * 2 * Mathf.PI;
        float nextAngle = arclength / entities.Count;
        float angle = 0;
        for (int i = 0; i < entities.Count; i++)
        {
            float x = startingPos.x + Mathf.Cos(angle) * radius * direction;
            float y = startingPos.y + Mathf.Sin(angle) * radius * direction;
            targetPos[i] = new Vector2(x, y);
            angle += nextAngle;
            count++;
        }
        return targetPos;
    }
}
