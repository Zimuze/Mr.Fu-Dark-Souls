using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermove : MonoBehaviour {

    [Header("----人物移动按键----")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";


    [Header("----镜头旋转按键----")]
    public string keyJUp;
    public string keyJDown;
    public string keyJLeft;
    public string keyJRight;


    [Header("----人物行为按键----")]
    public string keyrun = "space";
    public string keyjump= "f";
    public string keyattack="k";
    public string keyD;


    [Header("----output signals----")]

    public float Dup;
    public float Dright;
    public float Dmag;
    public Vector3 Dvec;

    //1.pressing signal
    public bool run;
    //2.trigger once signal
    //3.double trigger
    public bool jump;
    private bool lastjump;

    public bool attack;
    private bool lastAttack;

    
    public bool inputEnabled=true;


    private float tragetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;

    public float Jup;
    public float Jright;

    
    void Awake () {

        
    }
	
	void Update () {
    //    if (Input.GetKey(keyattack))
    //    {
    //        print("keyattack");
    //    }
    //    if (Input.GetKey(keyjump))
    //    {
    //        print("keyjump");
    //    }
        tragetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

        Jup= (Input.GetKey(keyJUp) ? 1.0f : 0) - (Input.GetKey(keyJDown) ? 1.0f : 0);
        Jright = (Input.GetKey(keyJRight) ? 1.0f : 0) - (Input.GetKey(keyJLeft) ? 1.0f : 0);


        if (inputEnabled == false)
        {
            tragetDup = 0;
            targetDright = 0;
        }

        Dup = Mathf.SmoothDamp(Dup, tragetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright,Dup));

        float Dright2 = tempDAxis.x;
        float Dup2 = tempDAxis.y;

        Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;

        run = Input.GetKey(keyrun);


        bool newJump = Input.GetKey(keyjump);
        
        if(newJump != lastjump && newJump == true)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
        lastjump = newJump;
        //attack状态

        bool newAttack = Input.GetKey(keyattack);

        if (newAttack != lastAttack && newAttack == true)
        {
            attack = true;
        }
        else
        {
            attack = false;
        }
        lastAttack = newAttack;
    }

    private Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;

    }


}
