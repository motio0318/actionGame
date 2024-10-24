using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    [SerializeField, Header("ゲームオーバー")]
    private GameObject gameOverUI;

    [SerializeField, Header("ゲームクリア")]
    private GameObject gameClearUI;


    private GameObject player;
    private bool bShoeUI;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerManagement>().gameObject;
        bShoeUI = false;
    }

    // Update is called once per frame
    void Update()
    {
        ShowGameOverUI();
    }

    private void ShowGameOverUI()
    {
        if (player != null) return;

        gameOverUI.SetActive(true);
        bShoeUI = true;
    }

    public  void ShowGameClearUI()
    {

        gameClearUI.SetActive(true);
        bShoeUI = true;

    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (!bShoeUI || !context.performed) return;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
