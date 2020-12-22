using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStructure : MonoBehaviour
{
    public Transform goalPoint;

    public Transform enemyPositionsParent;

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Color c = Gizmos.color;

        if (enemyPositionsParent != null)
        {
            Gizmos.color = Color.red;

            for (int i = 0; i < enemyPositionsParent.childCount; i++)
            {
                Transform enemy = enemyPositionsParent.GetChild(i);
                Gizmos.DrawCube(enemy.position, new Vector3(4f, 4f, 4f));
            }
        }


        if (goalPoint != null)
        {
            Gizmos.color = Color.cyan;

            Transform point = goalPoint.transform;
            Gizmos.DrawCube(point.position, new Vector3(40f, 100f, 10f));

            Gizmos.color = c;
        }
    }
#endif
}
