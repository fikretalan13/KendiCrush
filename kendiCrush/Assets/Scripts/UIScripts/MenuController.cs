using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public string levelAdi;

    public void OyunaBasla()
    {
        SceneManager.LoadScene(levelAdi);

    }

    public void OyundanCikis()
    {
        Application.Quit();
    }
}
