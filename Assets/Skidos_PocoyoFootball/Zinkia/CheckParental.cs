using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CheckParental : MonoBehaviour
{

    public Text textResult;
    public Text[] answers;
	public UnityEvent correctAnswer;
	public UnityEvent incorrectAnswer;

    private int buttonOKIndex;
    private int buttonOKNumber;
    private int buttonAnswer2;
    private int buttonAnswer3;

    public void show()
    {
        gameObject.SetActive(true);
        setResult();
    }

    public void setResult()
    {
        int randNum1 = Random.Range(1, 10);
        int randNum2 = Random.Range(1, 10);

        int randPosButtonAnswer = Random.Range(0, 3);
        buttonOKIndex = randPosButtonAnswer;

        int randOperation = Random.Range(0, 2);

        if (randOperation == 0) //sum
        {           
            int result = buttonOKNumber = randNum1 + randNum2;            
            answers[randPosButtonAnswer].text = result.ToString();

            textResult.text = randNum1 + " + " + randNum2;
        }
        else if (randOperation == 1) //resta
        {
            int result = -1;

            if (randNum1 >= randNum2)
            {
                result = buttonOKNumber = randNum1 - randNum2;
                textResult.text = randNum1 + " - " + randNum2;
            }
            else
            {
                result = buttonOKNumber = randNum2 - randNum1;
                textResult.text = randNum2 + " - " + randNum1;
            }

            answers[randPosButtonAnswer].text = result.ToString();
        }

        while (true)
        {
            if(buttonOKNumber > 10)
                buttonAnswer2 = Random.Range(1, 20);
            else
                buttonAnswer2 = Random.Range(1, 10);

            if (buttonAnswer2 != buttonOKNumber)
            {
                break;
            }
        }

        while (true)
        {
            if (buttonOKNumber > 10)
                buttonAnswer3 = Random.Range(1, 20);
            else
                buttonAnswer3 = Random.Range(1, 10);

            if (buttonAnswer3 != buttonOKNumber)
            {
                break;
            }
        }

        if (randPosButtonAnswer == 0)
        {
            answers[1].text = buttonAnswer2.ToString();
            answers[2].text = buttonAnswer3.ToString();
        }
        else if (randPosButtonAnswer == 1)
        {
            answers[0].text = buttonAnswer2.ToString();
            answers[2].text = buttonAnswer3.ToString();
        }
        else if (randPosButtonAnswer == 2)
        {
            answers[0].text = buttonAnswer2.ToString();
            answers[1].text = buttonAnswer3.ToString();
        }
    }

    public void checkParentalAnswer(int index)
    {
        if (index == buttonOKIndex)
        {
            gameObject.SetActive(false);
			correctAnswer.Invoke();
        }
        else
        {
            gameObject.SetActive(false);
			incorrectAnswer.Invoke();
        }

    }
}