using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{

    //int型を変数StageTipSizeで宣言します。ここの数値は自動生成したいオブジェクト端から端までの座標の距離の大きさ
    const int StageTipSize = 16;
    int currentTipIndex;

    //ターゲットキャラクターの指定ができる
    public Transform character;
    //ステージチップの配列
    public GameObject[] stageTips;
    //自動生成するときに使う変数startTipIndex
    public int startTipIndex;
    //ステージ生成の先読み個数
    public int preInstantiate;
    //作ったステージチップの保持リスト
    public List<GameObject> generateStageList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //初期化処理
        currentTipIndex = startTipIndex - 1;
        UpdateStage(preInstantiate);
    }

    // Update is called once per frame
    void Update()
    {
        //キャラクターの位置から現在のステージチップのインデックスを計算します
        int charaPositionIndex = (int)(character.position.x / StageTipSize);
        //次のステージチップに入ったらステージの更新処理を行います。
        if(charaPositionIndex + preInstantiate > currentTipIndex)
        {

            UpdateStage(charaPositionIndex + preInstantiate);

        }
    }


    //指定のインデックスまでのステージチップを生成して、管理下におく
    void UpdateStage(int toTipIndex)
    {

        if (toTipIndex <= currentTipIndex) return;
        //指定のステージチップまで生成するよ
        for(int i = currentTipIndex + 1; i <= toTipIndex; i++)
        {

            GameObject stageObject = GenerateStage(i);
            generateStageList.Add(stageObject);

        }
        while (generateStageList.Count > preInstantiate + 2) DestroyOldestStage();
        currentTipIndex = toTipIndex;

    }
    //指定のインデックス位置にstageオブジェクトをランダムに生成
    GameObject GenerateStage(int tipIndex)
    {

        int nextStageTip = Random.Range(0, stageTips.Length);

        GameObject stageObject = (GameObject)Instantiate(
            stageTips[nextStageTip],
            new Vector3(tipIndex * StageTipSize, 0, 0),//今回はx軸方向に無限生成するのでこの書き方をしている
            Quaternion.identity) as GameObject;
        return stageObject;

    }

    //一番古いステージを削除します
    void DestroyOldestStage()
    {

        GameObject oldStage = generateStageList[0];
        generateStageList.RemoveAt(0);
        Destroy(oldStage);

    }

}
