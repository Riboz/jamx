using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using System.Reflection;
using Unity.VisualScripting;


static public class LogicalInventory
{
    public enum ItemE
    {
        Null = -1,
        Cooler = 0,
        Heater = 1,
        Medicine = 2,
        Generator = 3

    }
    public class InventorySlot
    {
        public int count;
        public int index;
        public ItemE item;
        public InventorySlot()
        {
            this.count = 0;
            this.index = 0;
            this.item = ItemE.Null;
        }

    }

    static private InventorySlot[] activeInventory = new InventorySlot[4];
    static public void Init() 
    {
        for (int i = 0; i < 4; i++)
        {
            activeInventory[i] = new InventorySlot();
        }
    }
    static public void editItem(ItemE Item, int count, bool remove = false)
    {
        int index = -1;
        for (int i = 0; i < 4; i++)
        {
            if (activeInventory[i].item == ItemE.Null || activeInventory[i].item == Item)
            {
                index = i;
                activeInventory[i].item = Item;
                break;
            }
        }

        if (index == -1)
            return;

        activeInventory[index].index = index;

        if (remove)
            count *= -1;

        activeInventory[index].count = (activeInventory[index].count < (count * -1)) ? activeInventory[index].count : activeInventory[index].count + count;

        if (activeInventory[index].count == 0) 
        {
            activeInventory[index].item = ItemE.Null;
            activeInventory[index].index = index;
        }
    }

    static public InventorySlot getItemEnumfromInventory(int inventorySlotIndex)
    {
        if (-1 < inventorySlotIndex && inventorySlotIndex < 4)
            return activeInventory[inventorySlotIndex];
        else

            return new InventorySlot();          
    }

    static public int getItemIndex(ItemE item) 
    {
        for (int i = 0; i < 4; i++)
        {
            if (activeInventory[i].item == item)
            {
                return i;
            }
        }
        return -1;
    }
}



public class GameUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite Coolersp, Heatersp, Medicinesp, enginesp;
    [SerializeField] private Image FabricPanel, FabricButon, upperPanel;
    [SerializeField] private TMP_Text[] invCountText;
    private bool Open = false;
    public GameObject InventoryObj;
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

    private int[] ImageID = new int[4];
    private bool startDrag = false;

    public void Start()
    {
        StartCoroutine(ManufactoryRoutine());
        StartCoroutine(UpperPanelActivate());
        LogicalInventory.Init();

        invCountText[0].text = "0";
        invCountText[1].text = "0";
        invCountText[2].text = "0";
        invCountText[3].text = "0";

        for (int i = 0; i < 4; i++) 
        {
            switch (itemImages[i].name) 
            {
                case "Generator":
                    ImageID[(int)LogicalInventory.ItemE.Generator] = i;
                    break;

                case "Cooler":
                    ImageID[(int)LogicalInventory.ItemE.Cooler] = i;
                    break;

                case "Medicine":
                    ImageID[(int)LogicalInventory.ItemE.Medicine] = i;
                    break;

                case "Heater":
                    ImageID[(int)LogicalInventory.ItemE.Heater] = i;
                    break;
            }
        }

    }

    private void Update()
    {
        startDrag = true;


        if (Input.GetMouseButton(0) && startDrag)
        {
            Vector3 cursorPosition = Input.mousePosition;
            cursorPosition.z = 0f;
            Vector3 worldPosition = mainCam.ScreenToWorldPoint(cursorPosition);
            currentItemImage.transform.position = new Vector3(worldPosition.x, worldPosition.y, 0f);
            
            if (Input.GetMouseButtonUp(0))
            {
                startDrag = false;
                currentItemImage.gameObject.SetActive(false);
            }
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
            if (LogicalInventory.getItemIndex(LogicalInventory.ItemE.Heater) == -1) 
            {
                InventoryObj.GetComponent<InventorySc>().AddItem(Heatersp);
            }
            LogicalInventory.editItem(LogicalInventory.ItemE.Heater, 1);
            int index = LogicalInventory.getItemIndex(LogicalInventory.ItemE.Heater);
            invCountText[index].text = LogicalInventory.getItemEnumfromInventory(index).count.ToString();
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
            if (LogicalInventory.getItemIndex(LogicalInventory.ItemE.Cooler) == -1)
            {
                InventoryObj.GetComponent<InventorySc>().AddItem(Coolersp);
            }
            LogicalInventory.editItem(LogicalInventory.ItemE.Cooler, 1);
            int index = LogicalInventory.getItemIndex(LogicalInventory.ItemE.Cooler);
            invCountText[index].text = LogicalInventory.getItemEnumfromInventory(index).count.ToString();
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
            if (LogicalInventory.getItemIndex(LogicalInventory.ItemE.Generator) == -1)
            {
                InventoryObj.GetComponent<InventorySc>().AddItem(enginesp);
            }
            LogicalInventory.editItem(LogicalInventory.ItemE.Generator, 1);
            int index = LogicalInventory.getItemIndex(LogicalInventory.ItemE.Generator);
            invCountText[index].text = LogicalInventory.getItemEnumfromInventory(index).count.ToString();
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
            if (LogicalInventory.getItemIndex(LogicalInventory.ItemE.Medicine) == -1)
            {
                InventoryObj.GetComponent<InventorySc>().AddItem(Medicinesp);
            }
            LogicalInventory.editItem(LogicalInventory.ItemE.Medicine, 1);
            int index = LogicalInventory.getItemIndex(LogicalInventory.ItemE.Medicine);
            invCountText[index].text = LogicalInventory.getItemEnumfromInventory(index).count.ToString();
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
        if (Info >= 3)
        {
            chemicalTxt.gameObject.SetActive(false);
            chemicalMnButton.enabled = false;
            chemicalB = true;
            chemicalMnButton.GetComponentInChildren<PlanetRotation>().restart();
            Info -= 3;
            infoEnvt.text = Info + "/5";
        }
    }
    public void ManufacturingOre()
    {
        if (Info >= 4)
        {
            oreTxt.gameObject.SetActive(false);
            oreB = true;
            oreMnButton.enabled = false;
            oreMnButton.GetComponentInChildren<PlanetRotation>().restart();
            Info -= 4;
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
            yield return new WaitForSeconds(0.5f);

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
        if (itemImages[LogicalInventory.getItemEnumfromInventory(0).index])
        {
           
            if (currentItemImage.GetComponent<Image>())
            {
                currentItemImage.GetComponent<Image>().sprite = itemImages[ImageID[(int)LogicalInventory.getItemEnumfromInventory(0).item]];
                currentItemImage.gameObject.SetActive(true);
                startDrag = true;
                Debug.Log("if içi");

            }
        }
        Debug.Log(LogicalInventory.getItemEnumfromInventory(0).item.ToString());

    }
    public void GetItemSlot1()
    {
        //Determine which item it ha.
        if (itemImages[LogicalInventory.getItemEnumfromInventory(1).index])
        {
            currentItemImage.GetComponent<Image>().sprite = itemImages[ImageID[(int)LogicalInventory.getItemEnumfromInventory(1).item]];
            currentItemImage.gameObject.SetActive(true);
            startDrag = true;
            Debug.Log("if içi");
        }

        Debug.Log(LogicalInventory.getItemEnumfromInventory(1).item);

    }

    public void GetItemSlot2()
    {
        if (itemImages[LogicalInventory.getItemEnumfromInventory(2).index])
        {

            currentItemImage.GetComponent<Image>().sprite = itemImages[ImageID[(int)LogicalInventory.getItemEnumfromInventory(2).item]];
            currentItemImage.gameObject.SetActive(true);
            startDrag = true;
            Debug.Log("if içi");
        }

        Debug.Log(LogicalInventory.getItemEnumfromInventory(2).item.ToString());

    }
    public void GetItemSlot3()
    {
        if (itemImages[LogicalInventory.getItemEnumfromInventory(3).index])
        {
            currentItemImage.GetComponent<Image>().sprite = itemImages[ImageID[(int)LogicalInventory.getItemEnumfromInventory(3).item]];
            currentItemImage.gameObject.SetActive(true);
            startDrag = true;
            Debug.Log("if içi");
        }
        Debug.Log(LogicalInventory.getItemEnumfromInventory(3).item.ToString());

    }
}
