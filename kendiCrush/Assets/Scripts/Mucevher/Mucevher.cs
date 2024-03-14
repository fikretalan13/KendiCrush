using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mucevher : MonoBehaviour
{
    [HideInInspector]
    public Vector2Int posIndex;

    [HideInInspector]
    public Board board;

    public Vector2 birinciBasilanPos;
    public Vector2 sonBasilanPos;

    bool mouseBasildi;
    float suruklemeAcisi;

    Mucevher digerMucevher;

    public bool eslestimi;

    Vector2Int ilkPos;

    public GameObject mucevherEffect;

    public int bombaHacmi;

    public int skorDegeri;

    public enum MucehverTipi { mavi, pembe, sari, acikYesil, koyuYesil,bomba };
    public MucehverTipi tipi;


    private void Update()
    {

        if (Vector2.Distance(transform.position, posIndex) > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, posIndex, board.mucevherHiz * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(posIndex.x, posIndex.y, 0f);
        }


        if (mouseBasildi && Input.GetMouseButtonUp(0))
        {

            mouseBasildi = false;


            if (board.gecerliDurum == Board.BoardDurum.hareketEdiyor && !UIManager.Instance.turBittimi)
            {
                sonBasilanPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                AciyiHesaplaFNC();
            }
                

           
        }
    }
    public void MucehveriDuzenle(Vector2Int pos, Board theBoard)
    {
        posIndex = pos;
        board = theBoard;
    }

    private void OnMouseDown()
    {
        if (board.gecerliDurum==Board.BoardDurum.hareketEdiyor && !UIManager.Instance.turBittimi)
        {
            birinciBasilanPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseBasildi = true;
        }
      
    }

    void AciyiHesaplaFNC()
    {
        float dx = sonBasilanPos.x - birinciBasilanPos.x;
        float dy = sonBasilanPos.y - birinciBasilanPos.y;

        suruklemeAcisi = Mathf.Atan2(dy, dx);

        suruklemeAcisi = suruklemeAcisi * 180 / Mathf.PI;

        if (Vector3.Distance(birinciBasilanPos, sonBasilanPos) > 0.5f)
        {
            TileHareketFNC();
        }

    }

    void TileHareketFNC()
    {
        ilkPos = posIndex;
        if (suruklemeAcisi < 45 && suruklemeAcisi > -45 && posIndex.x < board.genislik - 1)
        {
            //Ýþaretlediðimiz mücevher 0,0 diger yanýndakki mucevher 1,0
            digerMucevher = board.tumMucevherler[posIndex.x + 1, posIndex.y];
            digerMucevher.posIndex.x--;
            posIndex.x++;
        }

        else if (suruklemeAcisi > 45 && suruklemeAcisi <= 135 && posIndex.y < board.yukseklik - 1)
        {
            //Ýþaretlediðimiz mücevher 0,0 diger yanýndakki mucevher 1,0
            digerMucevher = board.tumMucevherler[posIndex.x, posIndex.y + 1];
            digerMucevher.posIndex.y--;
            posIndex.y++;
        }

        else if (suruklemeAcisi < -45 && suruklemeAcisi >= -135 && posIndex.y > 0)
        {
            //Ýþaretlediðimiz mücevher 0,0 diger yanýndakki mucevher 1,0
            digerMucevher = board.tumMucevherler[posIndex.x, posIndex.y - 1];
            digerMucevher.posIndex.y++;
            posIndex.y--;
        }

        else if (suruklemeAcisi > 135 || suruklemeAcisi < -135 && posIndex.x > 0)
        {
            //Ýþaretlediðimiz mücevher 0,0 diger yanýndakki mucevher 1,0
            digerMucevher = board.tumMucevherler[posIndex.x - 1, posIndex.y];
            digerMucevher.posIndex.x++;
            posIndex.x--;
        }

        board.tumMucevherler[posIndex.x, posIndex.y] = this;
        board.tumMucevherler[digerMucevher.posIndex.x, digerMucevher.posIndex.y] = digerMucevher;

        StartCoroutine(HareketiKontrolEtRoutine());
    }

    public IEnumerator HareketiKontrolEtRoutine()
    {
        board.gecerliDurum = Board.BoardDurum.bekliyor;
        yield return new WaitForSeconds(0.5f);
        board.eslesmeController.EslesmeleriBulFNC();

        if (digerMucevher != null)
        {
            if (!eslestimi && !digerMucevher.eslestimi)
            {
                digerMucevher.posIndex = posIndex;
                posIndex = ilkPos;

                board.tumMucevherler[posIndex.x, posIndex.y] = this;
                board.tumMucevherler[digerMucevher.posIndex.x, digerMucevher.posIndex.y] = digerMucevher;

                yield return new WaitForSeconds(0.5f);
                board.gecerliDurum = Board.BoardDurum.hareketEdiyor;

            }
            else
            {
                board.TumEslesenleriYokEt();
            }
        }
    }
}
