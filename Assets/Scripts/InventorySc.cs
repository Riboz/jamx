using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class InventorySc : MonoBehaviour
{
    // Start is called before the first frame update
    public Image InventoryPanel;
    public bool Open;
    public Button InventoryButton;
    public Button[] EnvButtons;
    public Sprite[] EnvButtonImage;
    public static bool Envfull=false;
    [SerializeField] private TMP_Text[] invCountText;
    public void AddItem(Sprite ItemImage,int WhichItem)
    {
        int Counter=0;

        for(int i=0;i<EnvButtons.Length;i++)
        {
           
            if(EnvButtonImage[i]==null)
            {

                EnvButtonImage[i]=ItemImage;

                EnvButtons[i].GetComponent<ButtonItem>().WhichItem = WhichItem;

                EnvButtons[i].GetComponent<Image>().sprite=EnvButtonImage[i];

                EnvButtons[i].GetComponent<Image>().DOFade(1,0);

                break;
            }
            else
            {
                Counter+=1;

                if(Counter==4)
                {
                    Envfull=true;
                }
            }
        }
    }
    public void InventoryOpen()
    {
        switch(Open)
        {
            case false: InventoryPanel.transform.DOLocalMoveX(-756,0.5f).SetEase(Ease.OutElastic); InventoryButton.transform.DORotate(new Vector3(0,0,0),0.3f); break;
            case true: InventoryPanel.transform.DOLocalMoveX(-1163,0.5f).SetEase(Ease.OutElastic); InventoryButton.transform.DORotate(new Vector3(0,0,180),0.3f);break;
        }
       Open=!Open;
    }
    public void DeleteItem(int i)
    {
        switch(i)
        {
            case 0: 
                EnvButtonImage[0]=null;
                EnvButtons[0].GetComponent<Image>().sprite=null; 
                  EnvButtons[0].GetComponent<ButtonItem>().WhichItem=5; 
              
                invCountText[0].text = "0";
                EnvButtons[0].GetComponent<Image>().DOFade(0.1f,0); 
                
                break;
            case 1: 
                EnvButtonImage[1]=null;
                EnvButtons[1].GetComponent<Image>().sprite=null; 
              
                invCountText[1].text = "0";
                EnvButtons[1].GetComponent<Image>().DOFade(0.1f,0); 
                EnvButtons[1].GetComponent<ButtonItem>().WhichItem=5; 
                break;
            case 2: 
                EnvButtonImage[2]=null;
                EnvButtons[2].GetComponent<Image>().sprite=null; 
                
                invCountText[2].text = "0";
                EnvButtons[2].GetComponent<Image>().DOFade(0.1f,0);
                EnvButtons[2].GetComponent<ButtonItem>().WhichItem=5;  
                break;
            case 3: 
                EnvButtonImage[3]=null;
                EnvButtons[3].GetComponent<Image>().sprite=null; 
                EnvButtons[3].GetComponent<ButtonItem>().WhichItem=5; 
               
                invCountText[3].text = "0";
                EnvButtons[3].GetComponent<Image>().DOFade(0.1f,0); 
                break;
        }
        Envfull=false;
    }
    // inventoryde Bloklara sprite şeklinde koydum datayı yani if statement i sprite a ve b ise çalıştır gibi olabilir bu spritelar EnvButtonImagede(buttonların içindeki sprite da)
    // bu button sprite ları buttonların numarasıyla aynı sırada buttonımage[1] ve button[1] aynı buttonu işaret ediyor gibi düşünebilirsin
    
}
