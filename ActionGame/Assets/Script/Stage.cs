using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{

    //int�^��ϐ�StageTipSize�Ő錾���܂��B�����̐��l�͎��������������I�u�W�F�N�g�[����[�܂ł̍��W�̋����̑傫��
    const int StageTipSize = 16;
    int currentTipIndex;

    //�^�[�Q�b�g�L�����N�^�[�̎w�肪�ł���
    public Transform character;
    //�X�e�[�W�`�b�v�̔z��
    public GameObject[] stageTips;
    //������������Ƃ��Ɏg���ϐ�startTipIndex
    public int startTipIndex;
    //�X�e�[�W�����̐�ǂ݌�
    public int preInstantiate;
    //������X�e�[�W�`�b�v�̕ێ����X�g
    public List<GameObject> generateStageList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //����������
        currentTipIndex = startTipIndex - 1;
        UpdateStage(preInstantiate);
    }

    // Update is called once per frame
    void Update()
    {
        //�L�����N�^�[�̈ʒu���猻�݂̃X�e�[�W�`�b�v�̃C���f�b�N�X���v�Z���܂�
        int charaPositionIndex = (int)(character.position.x / StageTipSize);
        //���̃X�e�[�W�`�b�v�ɓ�������X�e�[�W�̍X�V�������s���܂��B
        if(charaPositionIndex + preInstantiate > currentTipIndex)
        {

            UpdateStage(charaPositionIndex + preInstantiate);

        }
    }


    //�w��̃C���f�b�N�X�܂ł̃X�e�[�W�`�b�v�𐶐����āA�Ǘ����ɂ���
    void UpdateStage(int toTipIndex)
    {

        if (toTipIndex <= currentTipIndex) return;
        //�w��̃X�e�[�W�`�b�v�܂Ő��������
        for(int i = currentTipIndex + 1; i <= toTipIndex; i++)
        {

            GameObject stageObject = GenerateStage(i);
            generateStageList.Add(stageObject);

        }
        while (generateStageList.Count > preInstantiate + 2) DestroyOldestStage();
        currentTipIndex = toTipIndex;

    }
    //�w��̃C���f�b�N�X�ʒu��stage�I�u�W�F�N�g�������_���ɐ���
    GameObject GenerateStage(int tipIndex)
    {

        int nextStageTip = Random.Range(0, stageTips.Length);

        GameObject stageObject = (GameObject)Instantiate(
            stageTips[nextStageTip],
            new Vector3(tipIndex * StageTipSize, 0, 0),//�����x�������ɖ�����������̂ł��̏����������Ă���
            Quaternion.identity) as GameObject;
        return stageObject;

    }

    //��ԌÂ��X�e�[�W���폜���܂�
    void DestroyOldestStage()
    {

        GameObject oldStage = generateStageList[0];
        generateStageList.RemoveAt(0);
        Destroy(oldStage);

    }

}
