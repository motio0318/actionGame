using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{

    [SerializeField, Header("HPÉAÉCÉRÉì")]
    private GameObject playerIcon;

    private PlayerManagement player;
    private int beforHP;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerManagement>();
        beforHP = player.GetHp();
        CreateHPIcon();
    }
    private void CreateHPIcon()
    {
        for(int i = 0;i < player.GetHp();i++)
        {
            GameObject playerHPObj = Instantiate(playerIcon);
            playerHPObj.transform.SetParent(transform);
        }

    }
    // Update is called once per frame
    void Update()
    {
        ShowHPIcon();
    }


    private void ShowHPIcon()
    {
        if (beforHP == player.GetHp()) return;

        Image[] icons = transform.GetComponentsInChildren<Image>();
        for(int i = 0; i < icons.Length;i++)
        {
            icons[i].gameObject.SetActive(i < player.GetHp());
        }
        beforHP = player.GetHp();
    }
}
