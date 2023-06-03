using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fill2048 : MonoBehaviour
{
    
    public int value;
    [SerializeField] private Text valueDisplay;
    [SerializeField] private float speed;

    private bool hasCombine;

    private Image myImage;

    public static bool canSpawn;
    public void FillValueUpdate(int valueIn)
    {
        value = valueIn;
        valueDisplay.text = value.ToString();

        //Change color
        int colorIndex = GetColorIndex(value);
        Debug.Log(colorIndex + " color index");
        myImage = GetComponent<Image>();
        myImage.color = GameController.instance.fillColors[colorIndex];

    }

    private int GetColorIndex(int valueIn)
    {
        int index = 0;
        while(valueIn != 1)
        {
            index++;
            valueIn /= 2;
        }

        index--;
        return index;
    } 

    private void Update()
    {
        
        if (transform.localPosition != Vector3.zero)
        {
            
            hasCombine = false;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, speed * Time.deltaTime);

        }
        else if (hasCombine == false)
        {
            if(transform.parent.GetChild(0) != this.transform)
            {
                Destroy(transform.parent.GetChild(0).gameObject);
            }
            hasCombine = true;
        }
    }

    public void Double()
    {
        value *= 2;
        GameController.instance.ScoreUpdate(value);
        valueDisplay.text = value.ToString();

        //Change color
        int colorIndex = GetColorIndex(value);
        Debug.Log(colorIndex + " color index");
        myImage.color = GameController.instance.fillColors[colorIndex];

        GameController.instance.WinningCheck(value);
    }

}
