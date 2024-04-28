using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject karePrefab;

    [SerializeField]
    private Transform karelerPaneli;

    private GameObject[] karelerDizisi=new GameObject[25];

    [SerializeField]
    private Transform soruPaneli;

    [SerializeField]
    private Text soruText;

    [SerializeField]
    private Sprite[] kareSprites;

    [SerializeField]
    private GameObject sonucPaneli;

    [SerializeField]
    AudioSource audioSource;

    List<int> bolumDegerleriListesi=new List<int>();

    int bolunenSayi, bolenSayi;
    int kacinciSoru;

    int butonDegeri;

    bool butonaBasilsinMi;
    int dogruSonuc;

    int kalanHak;

    string sorununZorlukDerecesi;

    GameObject gecerliKare;

    public AudioClip butonSesi;

    KalanHaklar kalanHaklar;
    PuanManager puanManager;

    private void Awake()
    {
        kalanHak = 3;
        audioSource = GetComponent<AudioSource>();
        puanManager = Object.FindObjectOfType<PuanManager>();
        kalanHaklar = Object.FindObjectOfType<KalanHaklar>();
        kalanHaklar.KalanHaklariKontrolEt(kalanHak);
        sonucPaneli.GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    void Start()
    {
        butonaBasilsinMi = false;

        soruPaneli.GetComponent<RectTransform>().localScale = Vector3.zero;

        KareleriOlustur();
    }

    public void KareleriOlustur()
    {
        for (int i = 0; i < karelerDizisi.Length; i++)
        {
            GameObject kare = Instantiate(karePrefab, karelerPaneli);
            kare.transform.GetChild(1).GetComponent<Image>().sprite = kareSprites[Random.Range(0, kareSprites.Length)];
            kare.transform.GetComponent<Button>().onClick.AddListener(() => ButonaBasildi());
            karelerDizisi[i] = kare;
        }

        BolumDegerleriniTexteYazdir();
        
        StartCoroutine(DoFadeRoutine());

        Invoke("SoruPaneliniAc", 2f);

    }

    void ButonaBasildi()
    {
        if(butonaBasilsinMi)
        {
            audioSource.PlayOneShot(butonSesi);

            butonDegeri = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text);

            gecerliKare = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

            Debug.Log(butonDegeri);

            SonucuKontrolEt();
        }

    }

    void SonucuKontrolEt()
    {

        if (butonDegeri == dogruSonuc)
        {
            gecerliKare.transform.GetChild(1).GetComponent<Image>().enabled = true;
            gecerliKare.transform.GetChild(0).GetComponent<Text>().text = "";
            gecerliKare.transform.GetComponent<Button>().interactable = false;

            puanManager.PuaniArtir(sorununZorlukDerecesi);

            bolumDegerleriListesi.RemoveAt(kacinciSoru);

            if(bolumDegerleriListesi.Count>0)
            {
                SoruPaneliniAc();
            }
            else
            {
                OyunBitti();
            }

            
        }
        else
        {
            kalanHak--;
            kalanHaklar.KalanHaklariKontrolEt(kalanHak);
        }

        if(kalanHak<=0)
        {
            OyunBitti();
        }

    }

    void OyunBitti()
    {
        butonaBasilsinMi = false;
        sonucPaneli.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack);
    }

    IEnumerator DoFadeRoutine()
    {
        foreach (var kare in karelerDizisi)
        {
            kare.GetComponent<CanvasGroup>().DOFade(1, 0.2f);

            yield return new WaitForSeconds(0.07f);
        }
    }

    void BolumDegerleriniTexteYazdir()
    {
        foreach (var kare in karelerDizisi)
        {
            int rastgeleDeger = Random.Range(1, 13);

            bolumDegerleriListesi.Add(rastgeleDeger);

            kare.transform.GetChild(0).GetComponent<Text>().text=rastgeleDeger.ToString();
        }

    }

    void SoruPaneliniAc()
    {
        SoruyuSor(); 
        butonaBasilsinMi = true;
        soruPaneli.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack);
    }

    void SoruyuSor()
    {
        bolenSayi = Random.Range(2, 11);

        kacinciSoru =Random.Range(0, bolumDegerleriListesi.Count);

        dogruSonuc = bolumDegerleriListesi[kacinciSoru];

        bolunenSayi = bolenSayi * bolumDegerleriListesi[kacinciSoru];

        if(bolunenSayi <=40)
        {
            sorununZorlukDerecesi = "kolay";
        }else if(bolunenSayi>40 && bolunenSayi<=80)
        {
            sorununZorlukDerecesi = "orta";
        }
        else
        {
            sorununZorlukDerecesi = "zor";
        }

        soruText.text=bolunenSayi.ToString()+" : "+bolenSayi.ToString();
    }

}
