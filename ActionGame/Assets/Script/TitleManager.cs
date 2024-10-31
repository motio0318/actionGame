using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class TitleManager : MonoBehaviour
{

    private bool bStart;
    private Fade fade;

    // Start is called before the first frame update
    void Start()
    {
        bStart = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeScene();
    }
    private void TitleStart()
    {
        bStart = true;
    }

    private void ChangeScene()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Initiate.Fade("SampleScene", Color.black, 1.0f);
        }

    }


}
