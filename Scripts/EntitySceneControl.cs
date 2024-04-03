using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySceneControl : MonoBehaviour
{
    public void StopAllEntitiesScene(Vector3 scenePosition)
    {
        GameObject scene = FindSceneByPosition(scenePosition);

        EnemyHealth[] enemyHealthArray = scene.GetComponentsInChildren<EnemyHealth>();
        NPCRandomPatrol[] NPCArray = scene.GetComponentsInChildren<NPCRandomPatrol>();
        foreach (EnemyHealth enemyHealth in enemyHealthArray)
        {
            enemyHealth.StopBehaviour();
        }
        foreach (NPCRandomPatrol npc in NPCArray)
        {
            npc.StopBehaviour();
        }
    }

    public void ActiveAllEntitiesScene(Vector3 scenePosition)
    {
        GameObject scene = FindSceneByPosition(scenePosition);

        EnemyHealth[] enemyHealthArray = scene.GetComponentsInChildren<EnemyHealth>();
        NPCRandomPatrol[] NPCArray = scene.GetComponentsInChildren<NPCRandomPatrol>();
        foreach (EnemyHealth enemyHealth in enemyHealthArray)
        {
            enemyHealth.ContinueBehaviour();
        }
        foreach (NPCRandomPatrol npc in NPCArray)
        {
            npc.ContinueBehaviour();
        }
    }

    public void ResetPositionEntitiesScene(Vector3 scenePosition)
    {
        GameObject scene = FindSceneByPosition(scenePosition);

        EnemyHealth[] enemyHealthArray = scene.GetComponentsInChildren<EnemyHealth>();
        NPCRandomPatrol[] NPCArray = scene.GetComponentsInChildren<NPCRandomPatrol>();
        foreach (EnemyHealth enemyHealth in enemyHealthArray)
        {
            enemyHealth.ResetPosition();
        }
        foreach (NPCRandomPatrol npc in NPCArray)
        {
            npc.ResetPosition();
        }
    }

    private GameObject FindSceneByPosition(Vector3 scenePosition)
    {
        foreach(Transform child in transform)
        {
            if (scenePosition.x == child.transform.position.x
                && scenePosition.y == child.transform.position.y)
            {
                return child.gameObject;
            }
        }

        return null;
    }
}
