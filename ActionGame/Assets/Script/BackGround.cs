using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{

    [SerializeField, Header("��������"), Range(0, 1)]
    private float parallxEffect;

    private GameObject camera;
    private float length;
    private float startPosX;

    // Start is called before the first frame update
    void Start()
    {
        startPosX = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        camera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Parallax();
    }

    private void Parallax()
    {
        float temp = camera.transform.position.x * (1 - parallxEffect);
        float dist = camera.transform.position.x * parallxEffect;

        transform.position = new Vector3(startPosX + dist, transform.position.y, transform.position.z);

        if(temp > startPosX + length)
        {
            startPosX += length;
        }
        else if (temp < startPosX - length)
        {
            startPosX -= length;
        }
    }
}
