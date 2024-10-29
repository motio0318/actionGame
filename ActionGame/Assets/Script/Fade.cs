using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Fade : MonoBehaviour
{

    //フェードの値を準備
    enum Mode
    {
        FadeIn,
        FadeOut,
    }

    [SerializeField, Header("フェードの時間")]
    private float fadeTime;

    [SerializeField, Header("フェードの種類")]
    private Mode mode;


    private bool bFade;//現在演出中かどうか判断する
    private float fadeCount;//フェード時間をカウントする
    private Image image;//用意したパネル画像を設定する
    private UnityEvent onFadeComplete = new UnityEvent();//


    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();//イメージ変数にイメージコンポーネントを代入

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
