using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushGameManager : MonoBehaviour
{
    [SerializeField] private List<FallingObjectSetting> fallingObjects; //ObjectPrefab
    [SerializeField] private GameTimer timer;       //時間管理Script
    private float[] spawnChance;                    //出現確率
    [SerializeField] private Transform spawnArea;   //出現場所
    [SerializeField] private float range;           //出現範囲
    [SerializeField] private float intervalMax;     //最大出現間隔
    private float interval;                         //出現間隔
    private float time;

    void Start()
    {
        //出現確率の配列を初期化
        spawnChance = new float[fallingObjects.Count];
        //出現間隔を代入
        interval = intervalMax;
        time = interval;
        //出現確率を代入
        for (int i = 0; i < fallingObjects.Count; i++)
        {
            spawnChance[i] = fallingObjects[i].spawnChance;
        }
    }

    void Update()
    {
        //時間計測
        time -= Time.deltaTime;
        if (time < 0 && timer.currentTime > 0)
        {
            //一定間隔で実行
            time = interval;
            IntervalChange();
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        //生成
        int index = Random.Range(0, 5);
        Vector3 spawnPos = new Vector3(spawnArea.position.x + Random.Range(-range, range), spawnArea.position.y + 10f, spawnArea.position.z + Random.Range(-range, range));
        Instantiate(fallingObjects[index].prefab, spawnPos, Quaternion.identity);
    }
    private void IntervalChange()
    {
        //出現間隔を調整
        for (int i = 0; i < timer.timeSet.Length; i++)
        {
            if (timer.currentTime < timer.timeSet[i])
            {
                interval = (intervalMax + interval) / 2 * timer.timeSet[i] / timer.timeLimit;
            }
        }
    }
}
