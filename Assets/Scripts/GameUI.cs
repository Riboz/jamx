using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Diagnostics;
public class GameUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private Image FabricPanel, FabricButon;
        private bool Open=false;
    public void FabricaOpen()
    {
        switch(Open)
        {
            case false: FabricPanel.transform.DOLocalMoveX(760,0.5f).SetEase(Ease.OutElastic); FabricButon.transform.DORotate(new Vector3(0,0,180),0.3f); break;
            case true: FabricPanel.transform.DOLocalMoveX(1165,0.5f).SetEase(Ease.OutElastic);  FabricButon.transform.DORotate(new Vector3(0,0,0),0.3f);break;
        }
       Open=!Open;
    }
}
