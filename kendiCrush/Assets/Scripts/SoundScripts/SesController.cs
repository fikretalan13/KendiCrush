using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SesController : MonoBehaviour
{
    public static SesController instance;

    public AudioSource mucevherSesi, patlamaSesi, oyunBitiSesi;


    private void Awake()
    {
        instance = this;
    }

    public void MucevherSesiCikar()
    {
        mucevherSesi.Stop();
        mucevherSesi.pitch = Random.Range(0.8f,1.2f);

        mucevherSesi.Play();
    }

    public void PatlamaSesiCikar()
    {
        patlamaSesi.Stop();
        patlamaSesi.pitch = Random.Range(0.8f, 1.2f);

        patlamaSesi.Play();
    }

    public void OyunBittiSesiCikar()
    {
        oyunBitiSesi.Play();
    }


}
