using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Hint : MonoBehaviour
{

    [SerializeField] private TMP_Text hintField;
    [SerializeField] private Transform hintPanelTransform;
    [SerializeField] private string[] hintTexts;
    [SerializeField] private Transform goToPosition;
    [SerializeField] private Image arrowImage;
    [SerializeField] private Button clickButton;

    private int initialPos = -345;
    private static int workingCount = 0;
    private static int sequenceCount = 0;

    private void Awake()
    {
        DOTween.Init();
        workingCount++;
    }

    private void Start()
    {


        if (workingCount == 1)
        {
            TutorialSequence();
        }
        

    }
 

    private void TutorialSequence()
    {
        //Produce knowledge first
        hintPanelTransform.DOLocalMoveY(goToPosition.position.y, 0.5f);

        switch (sequenceCount)
        {

            case 0:
                //Welcoming text.
                char[] currentChars = hintTexts[0].ToCharArray();
                for (int i = 0; i < currentChars.Length; i++)
                {
                    hintField.text += currentChars[i];
                }
                break;

            case 1:
                //Open panel.
                currentChars = hintTexts[1].ToCharArray();
                arrowImage.gameObject.SetActive(true);
                for (int i = 0; i < currentChars.Length; i++)
                {
                    hintField.text += currentChars[i];
                }

                break;


            case 2:
                //Bilgi üretmeye týkla.
                currentChars = hintTexts[2].ToCharArray();
                for (int i = 0; i < currentChars.Length; i++)
                {
                    hintField.text += currentChars[i];
                }

                arrowImage.gameObject.SetActive(false);

                break;
                
            case 3:
                //Paneli kapat
                hintField.text = "";
                hintPanelTransform.DOLocalMoveY(initialPos, 0.5f);
              
                break;
                

        }

    }

  

    private IEnumerator WriteTexts()
    {

        while (true)
        {
            hintField.text = "";
           
            hintPanelTransform.DOLocalMoveY(goToPosition.position.y, 0.5f);
            int randomTime = Random.Range(15, 95);
            int index = UnityEngine.Random.Range(3, hintTexts.Length);
            char[] currentChars = hintTexts[index].ToCharArray();

            for (int i = 0; i < currentChars.Length; i++)
            {

                hintField.text += currentChars[i];
                yield return new WaitForSeconds(0.01f);

            }

            yield return new WaitForSeconds(3f);
            hintPanelTransform.DOLocalMoveY(initialPos, 0.5f);

            yield return new WaitForSeconds(randomTime);
            hintField.text = "";
        }




    }

    public void IncreaseTutorialSequenceCount()
    {
         

        if (sequenceCount > 2)
        {
            int i = Random.Range(0, 4);
            hintField.text = "";
            hintPanelTransform.DOLocalMoveY(initialPos, 0.5f);

            if (i == 3)
            {
                hintField.text = "";
                StartCoroutine(WriteTexts());
            }

        }
        else
        {
            hintField.text = "";
            sequenceCount++;
            TutorialSequence();
        }
        

    }


}
