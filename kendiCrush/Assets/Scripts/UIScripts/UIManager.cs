using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    TMP_Text kalanZamanText;

    [SerializeField]
    TMP_Text skorText;

    [SerializeField]
    GameObject pausePanel;


    [SerializeField]
    GameObject turSonucPanel;

    public int kalanZaman = 60;

    public bool turBittimi;

    [HideInInspector]
    public int gecerliPuan;

    Board board;

    public string anaMenu;
    public string level1;

    private void Awake()
    {
        board=Object.FindAnyObjectByType<Board>();
        Instance = this;
    }
    private void Start()
    {
        turBittimi = false;
       StartCoroutine(GeriSayRoutine());
    }
    IEnumerator GeriSayRoutine()
    {
        while (kalanZaman>0)
        {
            yield return new WaitForSeconds(1f);

            kalanZamanText.text = kalanZaman.ToString() + " sn";
            kalanZaman--;
            if (kalanZaman<=0)
            {
                //oyun bitti
                
                SesController.instance.OyunBittiSesiCikar();
                turBittimi=true;
                turSonucPanel.SetActive(true);
            }
        }
    }

    public void PuaniArttirFNC(int gelenPuan)
    {
        gecerliPuan += gelenPuan;
        skorText.text = gecerliPuan + " Puan";
    }

    public void KaristirFNC()
    {
        board.BoardKaristir();
    }

    public void OyunuDurdurAc()
    {
        if (!pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;

        }

        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void AnaMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(anaMenu);
    }

    public void OyunaDonFNC()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void TekrarOyna()
    {
        SceneManager.LoadScene(level1);
    }
}
