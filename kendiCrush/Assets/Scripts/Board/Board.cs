using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int genislik;
    public int yukseklik;

    public GameObject tilePrefab;

    public Mucevher[] mucevherler;

    public Mucevher[,] tumMucevherler;

    public float mucevherHiz;
   public  EslesmeController eslesmeController;

    public enum BoardDurum { bekliyor,hareketEdiyor};

    public BoardDurum gecerliDurum = BoardDurum.hareketEdiyor;

    public Mucevher bomba;
    public float bombaCikmaSansi=2f;

    private void Awake()
    {
        eslesmeController = Object.FindAnyObjectByType<EslesmeController>();
    }

    private void Start()
    {
        tumMucevherler = new Mucevher[genislik,yukseklik];
        duzenleFnc();
    }
   


    void duzenleFnc()
    {
        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < yukseklik; y++)
            {
                Vector2 pos = new Vector2(x,y);
                
               GameObject bgTile= Instantiate(tilePrefab,pos,Quaternion.identity);
               bgTile.transform.parent=this.transform;
                bgTile.name = "BG Tile - " + x + ", " + y;

                int rastgeleMucevher = Random.Range(0,mucevherler.Length);

                int kontrolSayaci = 0;
                while (EslesmeVarmiFNC(new Vector2Int(x, y) , mucevherler[rastgeleMucevher]) && kontrolSayaci<100)
                {
                    rastgeleMucevher = Random.Range(0, mucevherler.Length);
                    kontrolSayaci++;
                    if (kontrolSayaci>0)
                    {
                        print(kontrolSayaci);
                    }
                }
                MucevherOlustur(new Vector2Int(x,y), mucevherler[rastgeleMucevher]);
            }
        }
    }

    void MucevherOlustur(Vector2Int pos, Mucevher olusacakMucevher)
    {
        if (Random.Range(0f,100f)<bombaCikmaSansi)
        {
            olusacakMucevher = bomba;
        }
        Mucevher mucevher = Instantiate(olusacakMucevher, new Vector3(pos.x,pos.y+yukseklik,0f), Quaternion.identity);
        mucevher.transform.parent=this.transform;
        mucevher.name="Mucevher - "+pos.x+ ", "+pos.y;

        tumMucevherler[pos.x,pos.y] = mucevher;

        mucevher.MucehveriDuzenle(pos,this);
    }

    bool EslesmeVarmiFNC(Vector2Int posKontrol, Mucevher kontrolEdilenMucevher)
    {
        if (posKontrol.x>1)
        {
            if (tumMucevherler[posKontrol.x-1,posKontrol.y].tipi==kontrolEdilenMucevher.tipi && tumMucevherler[posKontrol.x-2,posKontrol.y].tipi==kontrolEdilenMucevher.tipi)
            {
                return true;
            }
        }

        if (posKontrol.y > 1)
        {
            if (tumMucevherler[posKontrol.x, posKontrol.y-1].tipi == kontrolEdilenMucevher.tipi && tumMucevherler[posKontrol.x, posKontrol.y-2].tipi == kontrolEdilenMucevher.tipi)
            {
                return true;
            }
        }

        return false;
    }

    void EslesenMucevheriYokEt(Vector2Int pos)
    {
        if (tumMucevherler[pos.x, pos.y]!=null)
        {
            if (tumMucevherler[pos.x,pos.y].eslestimi)
            {
                if (tumMucevherler[pos.x,pos.y].tipi==Mucevher.MucehverTipi.bomba)
                {
                    SesController.instance.PatlamaSesiCikar();
                }
                else
                {
                    SesController.instance.MucevherSesiCikar();
                }

                Instantiate(tumMucevherler[pos.x, pos.y].mucevherEffect,new Vector2(pos.x,pos.y),Quaternion.identity);
                Destroy(tumMucevherler[pos.x, pos.y].gameObject);
                tumMucevherler[pos.x, pos.y] = null;
            }
        }
    }

    public void TumEslesenleriYokEt()
    {
        for (int i = 0; i < eslesmeController.BulunanMucevherlerListe.Count; i++)
        {
            if (eslesmeController.BulunanMucevherlerListe[i]!=null)
            {
                UIManager.Instance.PuaniArttirFNC(eslesmeController.BulunanMucevherlerListe[i].skorDegeri);
                EslesenMucevheriYokEt(eslesmeController.BulunanMucevherlerListe[i].posIndex);
            }
        }
        StartCoroutine(AltaKaydirRoutine());
    }

    IEnumerator AltaKaydirRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        int boslukSayac = 0;

        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < yukseklik; y++)
            {
                if (tumMucevherler[x,y]==null)
                {
                    boslukSayac++;
                }
                else if (boslukSayac>0)
                {
                    tumMucevherler[x, y].posIndex.y -= boslukSayac;
                    tumMucevherler[x, y - boslukSayac] = tumMucevherler[x, y];
                    tumMucevherler[x, y] = null;
                }
            }
            boslukSayac = 0;
        }
        StartCoroutine(BoardYenidenDoldurRoutine());
    }

    IEnumerator BoardYenidenDoldurRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        UstBosluklariDoldur();

        yield return new WaitForSeconds(0.5f);

        eslesmeController.EslesmeleriBulFNC();
        if (eslesmeController.BulunanMucevherlerListe.Count>0)
        {
            yield return new WaitForSeconds(0.7f);
            TumEslesenleriYokEt();
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            gecerliDurum = BoardDurum.hareketEdiyor;
        }

    }

    void UstBosluklariDoldur()
    {
        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < yukseklik; y++)
            {
                if (tumMucevherler[x,y]==null)
                {
                    int rastgeleMucevher = Random.Range(0, mucevherler.Length);
                    MucevherOlustur(new Vector2Int(x, y), mucevherler[rastgeleMucevher]);
                }
                
            }
        }
        YanlisYerlestirmeleriKontrolEt();

    }

    void YanlisYerlestirmeleriKontrolEt()
    {
        List<Mucevher> bulunanMucevherListesi = new List<Mucevher>();

        bulunanMucevherListesi.AddRange(FindObjectsOfType<Mucevher>());
        for (int x = 0; x < genislik; x++)
        {
            for (int y = 0; y < yukseklik; y++)
            {
                if (bulunanMucevherListesi.Contains(tumMucevherler[x,y]))
                {
                    bulunanMucevherListesi.Remove(tumMucevherler[x, y]);

                }
            }
        }
        foreach (Mucevher mucevher in bulunanMucevherListesi)
        {
            Destroy(mucevher.gameObject);
        }
    }

    public void BoardKaristir()
    {
        if (gecerliDurum != BoardDurum.bekliyor)
        {
            gecerliDurum = BoardDurum.bekliyor;

            List<Mucevher> sahnedekiMucevherlerList = new List<Mucevher>();
            for (int x = 0; x < genislik; x++)
            {
                for (int y = 0; y < yukseklik; y++)
                {
                    sahnedekiMucevherlerList.Add(tumMucevherler[x, y]);
                    tumMucevherler[x, y] = null;

                }
            }

            for (int x = 0; x < genislik; x++)
            {
                for (int y = 0; y < yukseklik; y++)
                {
                    int kullanilacakMucevher = Random.Range(0,sahnedekiMucevherlerList.Count);

                    int kontrolSayac = 0;

                    while (EslesmeVarmiFNC(new Vector2Int(x, y), sahnedekiMucevherlerList[kullanilacakMucevher])&&kontrolSayac<100&&sahnedekiMucevherlerList.Count>1)
                    {
                        kullanilacakMucevher = Random.Range(0, sahnedekiMucevherlerList.Count);
                        kontrolSayac++;
                    }

                    sahnedekiMucevherlerList[kullanilacakMucevher].MucehveriDuzenle(new Vector2Int(x, y), this);
                    tumMucevherler[x, y] = sahnedekiMucevherlerList[kullanilacakMucevher]; ;
                    sahnedekiMucevherlerList.RemoveAt(kullanilacakMucevher);
                }
            }
            StartCoroutine(AltaKaydirRoutine());
        }

    }
}
