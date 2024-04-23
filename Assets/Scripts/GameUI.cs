using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using System.Reflection;
using Unity.VisualScripting;


 
   

      

   

public class GameUI : MonoBehaviour
{
    // Start is called before the first frame update
        // cooler 0/ generator 1 /heater 2 /medicine 3
    public int[] ItemNum=new int[4];


    //The İnventory side is up
    public Sprite Coolersp, Heatersp, Medicinesp, enginesp;
    [SerializeField] private Image FabricPanel, FabricButon, upperPanel;
    
    private bool Open = false;
    public Button[] EnvButtons;
    
    public GameObject InventoryObj, disasterObj,Planet;
    public int Chemical, Electric, Info = 1, Ore;
    [SerializeField] private Image currentItemImage;
    [SerializeField] private Sprite[] itemImages;
    [SerializeField] private Camera mainCam;

    //Manufacturing Side
    public Button batteryMnButton, chemicalMnButton, oreMnButton, infoMnButton;
    bool batteryB = false, chemicalB = false, oreB = false, infoB = false;
    public TMP_Text btryTxt, chemicalTxt, oreTxt, infoTxt;
    [Header("Envanterdeki")]
    [SerializeField] private TMP_Text batteryEnvt, chemicalEnvt, oreEnvt, infoEnvt;

    public Sprite Mars,Karantina,Catlak,Donmuk;

    
    private bool startDrag = false;

    public void Start()
    {
        if(DisasterManager.Gezegen==0)
        {
            Planet.GetComponent<SpriteRenderer>().sprite=Karantina;
        }
       
         else if(DisasterManager.Gezegen==1)
        {
            Planet.GetComponent<SpriteRenderer>().sprite=Catlak;
        }
         else if(DisasterManager.Gezegen==2)
        {
            Planet.GetComponent<SpriteRenderer>().sprite=Mars;
        }
        else if(DisasterManager.Gezegen==3)
        {
            Planet.GetComponent<SpriteRenderer>().sprite=Donmuk;
        }
       
        StartCoroutine(ManufactoryRoutine());
        StartCoroutine(UpperPanelActivate());
      
        currentItemImage.gameObject.SetActive(false);

      

     

    }

    private void Update()
    {
        
        if (Input.GetMouseButton(0) && startDrag)
        {
            Vector3 cursorPosition = Input.mousePosition;
            cursorPosition.z = 0f;
            Vector3 worldPosition = mainCam.ScreenToWorldPoint(cursorPosition);
            currentItemImage.transform.position = new Vector3(worldPosition.x, worldPosition.y, 0f);       
        }
        else if (Input.GetMouseButtonUp(0) && startDrag)
        {
            startDrag = false;
            disasterObj.GetComponent<DisasterManager>().DistaterSolver(currentItemImage.GetComponent<ButtonItem>().WhichItem);
             currentItemImage.GetComponent<ButtonItem>().WhichItem = 5;
            currentItemImage.gameObject.SetActive(false);
        }

    }
    public void FabricaOpen()
    {
        switch (Open)
        {
            case false: FabricPanel.transform.DOLocalMoveX(760, 0.5f).SetEase(Ease.OutElastic); FabricButon.transform.DORotate(new Vector3(0, 0, 180), 0.3f); break;
            case true: FabricPanel.transform.DOLocalMoveX(1165, 0.5f).SetEase(Ease.OutElastic); FabricButon.transform.DORotate(new Vector3(0, 0, 0), 0.3f); break;
        }
        Open = !Open;
    }
    IEnumerator UpperPanelActivate()
    {
        yield return new WaitForSeconds(3f);
        upperPanel.transform.DOLocalMoveY(450, 0.5f).SetEase(Ease.OutElastic);

    }

    
    //Malzeme Üretme fnoksiyonları
    public void BuyHeater()
    {
        if (Chemical > 0 && Info > 0 && Ore > 0 && !InventorySc.Envfull)
        {
            Ore -= 1;
            Chemical -= 1;
            Info -= 1;
            infoEnvt.text = Info + "/5";
            batteryEnvt.text = Electric + "/3";
            oreEnvt.text = Ore + "/3";
           
           InventoryObj.GetComponent<InventorySc>().AddItem(Heatersp,2);

           
            //Üretilen malzeme Malzeme Deposuna gidicek
        }
        else
        {
            //kaynak olmadığını belirticek ses ve efekt
        }
    }
    public void BuyCooler()
    {
        if (Electric > 0 && Info > 0 && Ore > 0 && !InventorySc.Envfull)
        {
            Ore -= 1;
            Electric -= 1;
            Info -= 1;

            infoEnvt.text = Info + "/5";

            chemicalEnvt.text = Chemical + "/3";

            oreEnvt.text = Ore + "/3";
           
                InventoryObj.GetComponent<InventorySc>().AddItem(Coolersp,0);
           
            //Üretilen malzeme Malzeme Deposuna gidicek
        }
        else
        {
            //kaynak olmadığını belirticek ses ve efekt
        }
    }
    public void BuyElectric()
    {
        if (Electric > 0 && Info > 0 && !InventorySc.Envfull)
        {
            Electric -= 1;
            Info -= 1;
            infoEnvt.text = Info + "/5";
            batteryEnvt.text = Electric + "/3";
           
                InventoryObj.GetComponent<InventorySc>().AddItem(enginesp,1);
           
           
            //Üretilen malzeme Malzeme Deposuna gidicek
        }
        else
        {
            //kaynak olmadığını belirticek ses ve efekt
        }
    }
    public void BuyMedicine()
    {
        if (Chemical > 0 && Info > 0 && !InventorySc.Envfull)
        {
            Chemical -= 1;
            Info -= 1;
            infoEnvt.text = Info + "/5";
            chemicalEnvt.text = Chemical + "/3";
           
                InventoryObj.GetComponent<InventorySc>().AddItem(Medicinesp,3);
            
           
            //Üretilen malzeme Malzeme Deposuna gidicek
        }
        else
        {
            //kaynak olmadığını belirticek ses ve efekt
        }
    }

    public void ManufacturingBattery()
    {
        if (Info >= 2)
        {
            batteryB = true;
            btryTxt.gameObject.SetActive(false);
            batteryMnButton.enabled = false;
            batteryMnButton.GetComponentInChildren<PlanetRotation>().restart();
            Info -= 2;
            infoEnvt.text = Info + "/5";
        }

    }
    public void ManufacturingChemical()
    {
        if (Info >= 2)
        {
            chemicalTxt.gameObject.SetActive(false);
            chemicalMnButton.enabled = false;
            chemicalB = true;
            chemicalMnButton.GetComponentInChildren<PlanetRotation>().restart();
            Info -= 2;
            infoEnvt.text = Info + "/5";
        }
    }
    public void ManufacturingOre()
    {
        if (Info >=2)
        {
            oreTxt.gameObject.SetActive(false);
            oreB = true;
            oreMnButton.enabled = false;
            oreMnButton.GetComponentInChildren<PlanetRotation>().restart();
            Info -= 2;
            infoEnvt.text = Info + "/5";
        }
    }
    public void ManufacturingInfo()
    {

        if (Info >= 1)
        {
            infoTxt.gameObject.SetActive(false);
            infoB = true;
            infoMnButton.enabled = false;
            infoMnButton.GetComponentInChildren<PlanetRotation>().restart();
            Info -= 1;
            infoEnvt.text = Info + "/5";
        }
    }
    // aktif olan complar burada üretilir
    IEnumerator ManufactoryRoutine()
    {
        while (true)
        {
            //TODO change it back to 5
            yield return new WaitForSeconds(3f);

            if (infoB && Info < 5)
            {
                Info += 1;
                infoEnvt.text = Info + "/5";
            }

            if (oreB && Ore < 3)
            {
                Ore += 1;
                oreEnvt.text = Ore + "/3";
            }

            if (chemicalB && Chemical < 3)
            {
                Chemical += 1;
                chemicalEnvt.text = Chemical + "/3";
            }

            if (batteryB && Electric < 3)
            {
                Electric += 1;
                batteryEnvt.text = Electric + "/3";
            }
        }

    }


    public void GetItemSlot0()
    {
        //Determine which item it ha.
       
       
            // sprite yoksa veyahut buttonun whicitemi 5 ise get item olmasın
            if(EnvButtons[0].GetComponent<ButtonItem>().WhichItem != 5 )
            {
                currentItemImage.GetComponent<Image>().sprite = EnvButtons[0].GetComponent<Image>().sprite;
                currentItemImage.GetComponent<ButtonItem>().WhichItem = EnvButtons[0].GetComponent<ButtonItem>().WhichItem;
                // current image in icine rakamımızı which item seyini atalım butonumuzun icindeki
                currentItemImage.gameObject.SetActive(true);
                startDrag = true;
                // itemimizin kalan sayısı kadar
                  InventoryObj.GetComponent<InventorySc>().EnvButtonImage[0]=null;
            InventoryObj.GetComponent<InventorySc>(). EnvButtons[0].GetComponent<Image>().sprite=null; 
                    EnvButtons[0].GetComponent<ButtonItem>().WhichItem = 5 ;
                   
                    EnvButtons[0].GetComponent<Image>().sprite = null;
                    EnvButtons[0].GetComponent<Image>().DOFade(0.05f, 0.1f);
            }
            
        }
        
  
    public void GetItemSlot1()
    {
        //Determine which item it ha.
       // sprite yoksa veyahut buttonun whicitemi 5 ise get item olmasın
          if(EnvButtons[1].GetComponent<ButtonItem>().WhichItem != 5 )
          {
           currentItemImage.GetComponent<Image>().sprite = EnvButtons[1].GetComponent<Image>().sprite;
            currentItemImage.GetComponent<ButtonItem>().WhichItem = EnvButtons[1].GetComponent<ButtonItem>().WhichItem;
            // current image in icine rakamımızı which item seyini atalım butonumuzun icindeki
            currentItemImage.gameObject.SetActive(true);
            startDrag = true;
            EnvButtons[1].GetComponent<ButtonItem>().WhichItem = 5 ;
           //itemin kalan sayısı kadar
                InventoryObj.GetComponent<InventorySc>().EnvButtonImage[1]=null;
                InventoryObj.GetComponent<InventorySc>(). EnvButtons[1].GetComponent<Image>().sprite=null; 
                
                EnvButtons[1].GetComponent<Image>().sprite = null;
                EnvButtons[1].GetComponent<Image>().DOFade(0.05f, 0.1f);
    }
        }
        
       
    

    public void GetItemSlot2()
    {
             // sprite yoksa veyahut buttonun whicitemi 5 ise get item olmasın
                if(EnvButtons[2].GetComponent<ButtonItem>().WhichItem != 5 )
                {
            currentItemImage.GetComponent<Image>().sprite = EnvButtons[2].GetComponent<Image>().sprite;
             currentItemImage.GetComponent<ButtonItem>().WhichItem = EnvButtons[2].GetComponent<ButtonItem>().WhichItem;
            // current image in icine rakamımızı which item seyini atalım butonumuzun icindeki
            currentItemImage.gameObject.SetActive(true);
            startDrag = true;
            InventoryObj.GetComponent<InventorySc>().EnvButtonImage[2]=null;
            InventoryObj.GetComponent<InventorySc>(). EnvButtons[2].GetComponent<Image>().sprite=null; 
            EnvButtons[2].GetComponent<ButtonItem>().WhichItem = 5 ;
          
            EnvButtons[2].GetComponent<Image>().sprite = null;
            EnvButtons[2].GetComponent<Image>().DOFade(0.05f, 0.1f);
            
             }
        
    }
    public void GetItemSlot3()
    {
        
           if(EnvButtons[3].GetComponent<ButtonItem>().WhichItem != 5 )
           {

          
            currentItemImage.GetComponent<Image>().sprite = EnvButtons[3].GetComponent<Image>().sprite;
             currentItemImage.GetComponent<ButtonItem>().WhichItem = EnvButtons[3].GetComponent<ButtonItem>().WhichItem;
           // current image in icine rakamımızı which item seyini atalım butonumuzun icindeki
            currentItemImage.gameObject.SetActive(true);
            startDrag = true;
                  InventoryObj.GetComponent<InventorySc>().EnvButtonImage[3]=null;
            InventoryObj.GetComponent<InventorySc>(). EnvButtons[3].GetComponent<Image>().sprite=null; 
              EnvButtons[3].GetComponent<ButtonItem>().WhichItem = 5 ;
                
                EnvButtons[3].GetComponent<Image>().sprite = null;
                EnvButtons[3].GetComponent<Image>().DOFade(0.05f, 0.1f);
            
             }
        
    }
}
