using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{


    [SerializeField, Header("U“®ŠÔ")]
    private float shakeTime;
    [SerializeField, Header("U“®‚Ì‘å‚«‚³")]
    private float shakeMagnitude;




    private PlayerManagement player;
    private Vector3 initPos;
    private float shakeCount;
    private int currentPlayerHP;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerManagement>();
        currentPlayerHP = player.GetHp();
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ShakeCheck();
        FollowPlayer();
    }

    private void ShakeCheck()
    {
        if(currentPlayerHP != player.GetHp())
        {
            currentPlayerHP = player.GetHp();
            shakeCount = 0.0f;
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        Vector3 initPos = transform.position;

        while(shakeCount < shakeTime)
        {

            float x = initPos.x + Random.Range(-shakeTime, shakeMagnitude);
            float y = initPos.y + Random.Range(-shakeTime, shakeMagnitude);
            transform.position = new Vector3(x, y, initPos.z);

            shakeCount += Time.deltaTime;

            yield return null;
        }

        transform.position = initPos;
    }

    private void FollowPlayer()
    {
        if (player == null) return;

        float x = player.transform.position.x;
        x = Mathf.Clamp(x, initPos.x, Mathf.Infinity);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}
