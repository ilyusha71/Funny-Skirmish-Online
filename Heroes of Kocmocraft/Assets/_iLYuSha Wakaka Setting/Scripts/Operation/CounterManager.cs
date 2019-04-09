using UnityEngine;

public class CounterManager : MonoBehaviour
{
    public class DigitDisplay
    {
        public Renderer[] LEDs = new Renderer[7];
        public bool[,] array = new bool[11, 7]
        {
            {true,true,true,true,false,true,true}, // 0
            {false,true,false,true,false,false,false}, // 1
            {false,true,true,false,true,true,true}, // 2
            {false,true,false,true,true,true,true}, // 3
            {true,true,false,true,true,false,false}, // 4
            {true,false,false,true,true,true,true}, // 5
            {true,false,true,true,true,true,true}, // 6
            {false,true,false,true,false,true,false}, // 7
            {true,true,true,true,true,true,true}, // 8
            {true,true,false,true,true,true,true}, // 9
            {false,false,false,false,true,false,false} // -
        };

        public void DisplayNumber(int number)
        {
            for (int i = 0; i < LEDs.Length; i++)
            {
                LEDs[i].enabled = array[number, i];
            }
        }
    }
    public DigitDisplay hundred = new DigitDisplay();
    public DigitDisplay ten = new DigitDisplay();
    public DigitDisplay one = new DigitDisplay();

    void Start()
    {
        Renderer[] lampole = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < lampole.Length; i++)
        {
            if (i < 7)
                hundred.LEDs[i] = lampole[i];
            else if (i < 14)
                ten.LEDs[i - 7] = lampole[i];
            else
                one.LEDs[i - 14] = lampole[i];
        }
        hundred.DisplayNumber(10);
        ten.DisplayNumber(10);
        one.DisplayNumber(10);
    }
    public void ShowNumber(int number)
    {
        hundred.DisplayNumber(number / 100);
        ten.DisplayNumber((number % 100) / 10);
        one.DisplayNumber(number % 10);
    }
}
