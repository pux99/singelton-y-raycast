using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Material test;
    public int colores;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (colores)
        {
            case 0:
                test.color = Color.cyan;
                break;
            case 1:
                test.color = Color.magenta;
                break;
            case 2:
                test.color = Color.red;
                break;
            case 3:
                test.color = Color.black;
                break;
            case 4:
                test.color = Color.blue;
                break;
            case 5:
                test.color = Color.yellow;
                break;
            case 6:
                test.color = Color.green;
                break;
            case 7:
                test.color = Color.grey;
                break;
            case 8:
                test.color = Color.cyan;
                break;
            default:
                break;
        }
     }
}
