using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushGameManager : MonoBehaviour
{
    [SerializeField] private ChangeScene scene;             //Scene管理スクリプト
    [SerializeField] private PlayerHealth hp;               //HP管理スクリプト
    [SerializeField] private GameTimer timer;               //時間管理スクリプト
    [SerializeField] private GameObject player;             //プレイヤーオブジェクト
    [SerializeField] private GameObject[] fallingObjects;   //落ち物オブジェクト
    [SerializeField] private Transform spawnArea;           //出現場所
    [SerializeField] private float range;                   //出現範囲
    [SerializeField] private float intervalMax;             //最大出現間隔
    [SerializeField] private float intervalMultiply;        //出現間隔変化倍率
    private float interval;                                 //出現間隔
    private float time;

    void Start()
    {
        //出現間隔を代入
        interval = intervalMax;
        time = interval;
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
            //ランダム生成
            Vector3 spawnPos = new Vector3(spawnArea.position.x + Random.Range(-range, range), spawnArea.position.y + 20f, spawnArea.position.z + Random.Range(-range, range));
            SpawnObject(Random.Range(0, fallingObjects.Length), spawnPos);
            if (Random.Range(0, 1f) < (interval / intervalMax))
            {
                //プレイヤー位置を取得して生成
                Vector3 playerPos = player.transform.position;
                playerPos.y += 20f;
                SpawnObject(Random.Range(0, fallingObjects.Length), playerPos);
            }
        }

        //ゲームオーバー
        if (hp.hp <= 0) scene.LoadScene("GameOverScene");
        //ゲームクリア
        if (timer.currentTime < 0) scene.LoadScene("GameClearScene");
    }

    private void SpawnObject(int index, Vector3 pos)
    {
        Instantiate(fallingObjects[index], pos, fallingObjects[index].transform.rotation);
    }
    private void IntervalChange()
    {
        //出現間隔を調整
        for (int i = 0; i < timer.timeSet.Length; i++)
        {
            if (timer.currentTime < timer.timeSet[i])
            {
                interval = ((intervalMax + interval) * (timer.timeSet[i] / timer.timeLimit)) * intervalMultiply;
            }
        }
    }
}
