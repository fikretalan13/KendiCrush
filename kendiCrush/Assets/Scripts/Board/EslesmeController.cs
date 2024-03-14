using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.ComponentModel.Design;

public class EslesmeController : MonoBehaviour
{
    Board board;

    public List<Mucevher> BulunanMucevherlerListe = new List<Mucevher>();
    private void Awake()
    {
        board = Object.FindAnyObjectByType<Board>();
    }

    public void EslesmeleriBulFNC()
    {
        BulunanMucevherlerListe.Clear();
        for (int x = 0; x < board.genislik; x++)
        {
            for (int y = 0; y < board.yukseklik; y++)
            {
                Mucevher gecerliMucevher = board.tumMucevherler[x, y];
                if (gecerliMucevher != null)
                {
                    //X sýrasýndaki eþleþmeleri kontrol edildi
                    if (x > 0 && x < board.genislik - 1)
                    {
                        Mucevher solMucevher = board.tumMucevherler[x - 1, y];
                        Mucevher sagMucevher = board.tumMucevherler[x + 1, y];

                        if (solMucevher != null && sagMucevher != null)
                        {
                            if (solMucevher.tipi == gecerliMucevher.tipi && sagMucevher.tipi == gecerliMucevher.tipi)
                            {
                                gecerliMucevher.eslestimi = true;
                                solMucevher.eslestimi = true;
                                sagMucevher.eslestimi = true;
                                BulunanMucevherlerListe.Add(gecerliMucevher);
                                BulunanMucevherlerListe.Add(solMucevher);
                                BulunanMucevherlerListe.Add(sagMucevher);
                            }
                        }
                    }
                    //y sýrasýndaki eþleþmeler kontrol edildi

                    if (y > 0 && y < board.genislik - 1)
                    {
                        Mucevher altMucevher = board.tumMucevherler[x, y - 1];
                        Mucevher ustMucevher = board.tumMucevherler[x, y + 1];

                        if (altMucevher != null && ustMucevher != null)
                        {
                            if (altMucevher.tipi == gecerliMucevher.tipi && ustMucevher.tipi == gecerliMucevher.tipi)
                            {
                                gecerliMucevher.eslestimi = true;
                                altMucevher.eslestimi = true;
                                ustMucevher.eslestimi = true;
                                BulunanMucevherlerListe.Add(gecerliMucevher);
                                BulunanMucevherlerListe.Add(altMucevher);
                                BulunanMucevherlerListe.Add(ustMucevher);
                            }
                        }
                    }
                }
            }
        }//döngüler bitti
        if (BulunanMucevherlerListe.Count > 0)
        {
            BulunanMucevherlerListe = BulunanMucevherlerListe.Distinct().ToList();
        }
        BombayiBulFNC();

    }
    //önce bombayý buluyoruz
    public void BombayiBulFNC()
    {
        for (int i = 0; i < BulunanMucevherlerListe.Count; i++)
        {
            Mucevher mucevher = BulunanMucevherlerListe[i];
            int x = mucevher.posIndex.x;
            int y = mucevher.posIndex.y;

            if (mucevher.posIndex.x > 0)
            {
                if (board.tumMucevherler[x - 1, y] != null)
                {
                    if (board.tumMucevherler[x - 1, y].tipi == Mucevher.MucehverTipi.bomba)
                    {
                        BombaBolgesiniIsaretle(new Vector2Int(x - 1, y), board.tumMucevherler[x-1,y]);
                    }
                }
            }


            if (mucevher.posIndex.x < board.genislik-1)
            {
                if (board.tumMucevherler[x + 1, y] != null)
                {
                    if (board.tumMucevherler[x + 1, y].tipi == Mucevher.MucehverTipi.bomba)
                    {
                        BombaBolgesiniIsaretle(new Vector2Int(x + 1, y), board.tumMucevherler[x + 1, y]);
                    }
                }
            }

            if (mucevher.posIndex.y > 0)
            {
                if (board.tumMucevherler[x , y-1] != null)
                {
                    if (board.tumMucevherler[x , y-1].tipi == Mucevher.MucehverTipi.bomba)
                    {
                        BombaBolgesiniIsaretle(new Vector2Int(x, y-1), board.tumMucevherler[x , y-1]);
                    }
                }
            }

            if (mucevher.posIndex.y < board.yukseklik-1)
            {
                if (board.tumMucevherler[x, y + 1] != null)
                {
                    if (board.tumMucevherler[x, y + 1].tipi == Mucevher.MucehverTipi.bomba)
                    {
                        BombaBolgesiniIsaretle(new Vector2Int(x, y + 1), board.tumMucevherler[x, y + 1]);
                    }
                }
            }
        }
    }

    public void BombaBolgesiniIsaretle(Vector2Int bombaPos, Mucevher bomba)
    {
        for (int x = bombaPos.x - bomba.bombaHacmi; x <= bombaPos.x + bomba.bombaHacmi; x++)
        {
            for (int y = bombaPos.y - bomba.bombaHacmi; y < bombaPos.y + bomba.bombaHacmi; y++)
            {
                if (x >= 0 && x < board.genislik - 1 && y >= 0 && y < board.yukseklik - 1)
                {
                    if (board.tumMucevherler[x, y] != null)
                    {
                        board.tumMucevherler[x, y].eslestimi = true;
                        BulunanMucevherlerListe.Add(board.tumMucevherler[x,y]);
                    }

                }
            }
        }
        if (BulunanMucevherlerListe.Count > 0)
        {
            BulunanMucevherlerListe = BulunanMucevherlerListe.Distinct().ToList();
        }
    }
}
