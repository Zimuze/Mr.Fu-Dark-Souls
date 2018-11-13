using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerCollroer : MonoBehaviour {

    public Playermove pi;
    public float horizontalSpeed ;
    public float verticalSpeed ;
    [Header("----镜头灵敏度----")]
    public float cameraDampValue ;

    private GameObject PlayerHandle;
    private GameObject cameraHandle;
    private float tempEulerX;
    private GameObject model;
    private GameObject cameraMove;
    
    private Vector3 cameraDampVelocity;

	// Use this for initialization
	void Awake () {
       
        cameraHandle = transform.parent.gameObject;
        PlayerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerX = 20;
        model = PlayerHandle.GetComponent<Animacoll>().model;
        cameraMove = Camera.main.gameObject;
	}
	 
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 tempModelEuler = model.transform.eulerAngles;

        PlayerHandle.transform.Rotate(Vector3.up, pi.Jright * horizontalSpeed * Time.fixedDeltaTime);
        // cameraHandle.transform.Rotate(Vector3.right, pi.Jup * verticalSpeed * Time.deltaTime);
        //tempEulerX= cameraHandle.transform.eulerAngles.x;
        tempEulerX -= pi.Jup * -verticalSpeed * Time.fixedDeltaTime;
        tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);
        cameraHandle.transform.localEulerAngles=new Vector3(tempEulerX,0,0);

        model.transform.eulerAngles = tempModelEuler;

        //cameraMove.transform.position = Vector3.Lerp(cameraMove.transform.position, transform.position, 0.2f);
        cameraMove.transform.position = Vector3.SmoothDamp(cameraMove.transform.position,transform.position, ref cameraDampVelocity, cameraDampValue);
        cameraMove.transform.eulerAngles = transform.eulerAngles;

    }

}
