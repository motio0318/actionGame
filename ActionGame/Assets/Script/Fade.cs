using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Fade : MonoBehaviour
{

    //�t�F�[�h�̒l������
    enum Mode
    {
        FadeIn,
        FadeOut,
    }

    [SerializeField, Header("�t�F�[�h�̎���")]
    private float fadeTime;

    [SerializeField, Header("�t�F�[�h�̎��")]
    private Mode mode;


    private bool bFade;//���݉��o�����ǂ������f����
    private float fadeCount;//�t�F�[�h���Ԃ��J�E���g����
    private Image image;//�p�ӂ����p�l���摜��ݒ肷��
    private UnityEvent onFadeComplete = new UnityEvent();//


    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();//�C���[�W�ϐ��ɃC���[�W�R���|�[�l���g����

        switch(mode)
        {
            case Mode.FadeIn : fadeCount = fadeTime;break;
            case Mode.FadeOut: fadeCount = 0; break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        Fadep();
    }

    private void Fadep()
    {
        if (!bFade) return;

        switch(mode)
        {
            case Mode.FadeIn :break;
            case Mode.FadeOut:break;

        }
        float alpha = fadeCount / fadeTime;
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);


    }

    private void FadeIn()
    {
        fadeCount -= Time.deltaTime;
        if(fadeCount <= 0)
        {
            mode = Mode.FadeOut;
            bFade = false;
            onFadeComplete.Invoke();
        }
    }

    private void FadeOut()
    {
        fadeCount += Time.deltaTime;
        if(fadeCount >= fadeTime)
        {
            mode = Mode.FadeIn;
            bFade = false;
            onFadeComplete.Invoke();
        }
    }

    public void FadeStart(UnityAction listener)
    {
        if (bFade) return;
        bFade = true;
        onFadeComplete.AddListener(listener);
    }

}
