using UnityEngine;
using System;
using System.Collections.Generic;

public class CheckPointGenerator : MonoBehaviour
{
    [Header("チェックポイントの設定")]
    [SerializeField] private GameObject checkPointPrefab;

    [Header("スポーン範囲")]
    [SerializeField] private SpawnArea[] spawnAreas = new SpawnArea[4];

    [Header("NG座標リスト")]
    [SerializeField] private List<BoxCollider> ngAreas = new List<BoxCollider>();

    public event Action<CheckPoint> OnCheckPointSpawned;

    public void Setup()
    {
        SpawnAllCheckPoints();
    }

    private void SpawnAllCheckPoints()
    {
        foreach(SpawnArea area in spawnAreas)
        {
            SpawnRandomCheckPoint(area);
        }
    }

    private void SpawnRandomCheckPoint(SpawnArea area)
    {
        if (checkPointPrefab == null) return;

        while (true)
        {
            Vector3 candidate = area.GetRandomPosition();

            // NG範囲に含まれていないかチェック
            bool isInsideNG = false;
            foreach (var ng in ngAreas)
            {
                if (ng.bounds.Contains(candidate))
                {
                    isInsideNG = true;
                    break;
                }
            }

            if (!isInsideNG)
            {
                GameObject checkPointObject = Instantiate(checkPointPrefab, candidate, Quaternion.identity);
                CheckPoint checkPoint = checkPointObject.GetComponent<CheckPoint>();

                if (checkPoint != null)
                {
                    OnCheckPointSpawned?.Invoke(checkPoint);
                }

                return;
            }
        }
    }
}

[System.Serializable]
public struct SpawnArea
{
    public Vector3 min;
    public Vector3 max;

    public Vector3 GetRandomPosition()
    {
        return new Vector3(
            UnityEngine.Random.Range(min.x, max.x),
            UnityEngine.Random.Range(min.y, max.y),
            UnityEngine.Random.Range(min.z, max.z)
        );
    }
}
