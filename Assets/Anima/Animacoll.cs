using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animacoll : MonoBehaviour {

    public GameObject model;
    public Playermove pl;


    public float walkspeed=1.4f;//因为存在动画的播放问题，所以推荐设置1.4f更圆滑一点
    public float runspeed = 2.0f;
    public float jumphigh = 4.0f;
    public float rollSpeed = 1.0f;
    public float jabspeed ;


    [Header("==Friction Settings==")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;


    private Animator anima;
    private Rigidbody rigid;
    private Vector3 planarVec;
    private Vector3 thrustVec;
    private bool canAttack=true;

    private bool lockPlanar = false;

    private CapsuleCollider col;
    private float lerpTarget;



    void Start () {
        pl = GetComponent<Playermove>();
        anima = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        col = GetComponent<CapsuleCollider>();
    }
	
	void Update () {

        
        anima.SetFloat("forward",pl.Dmag * Mathf.Lerp(anima.GetFloat("forward"),((pl.run) ? 2.0f: 1.0f),0.5f));

        if (rigid.velocity.magnitude > 1.0f)//进入翻滚状态的高度
        {
            anima.SetTrigger("roll");
        }

        if (pl.jump) {
            anima.SetTrigger("jump");
        }
     
        //   if(pl.attack && CheckState("ground") && canAttack)
        //   {
        //       anima.SetTrigger("attack");
        //   }
        if (pl.attack && CheckState("ground") && canAttack)
        {
            anima.SetTrigger("attack");
            canAttack = true;
        }


        if (pl.Dmag > 0.1f)//使计算的键盘输入的向量没有方向的时候不会自动重置
        {
            model.transform.forward = Vector3.Slerp(model.transform.forward, pl.Dvec, 0.2f);
        }

        if (lockPlanar == false)
        {
            planarVec = pl.Dmag * model.transform.forward * walkspeed * ((pl.run) ? runspeed : 1.0f);//run为ture返回自定义的值，false返回1.0f
        }

        //print(CheckState("idle","attack"));
     
    }
    

    void FixedUpdate () {//物理引擎中计算“刷新”并计算
        rigid.position += planarVec * Time.fixedDeltaTime * walkspeed ;
        rigid.velocity += thrustVec;
        thrustVec = Vector3.zero;
    }

    private bool CheckState(string stateName,string layerName = "Base Layer")
    {
        //return anima.GetCurrentAnimatorStateInfo(anima.GetLayerIndex(layerName)).IsName(stateName); 
        int layerIndex = anima.GetLayerIndex(layerName);
        bool result = anima.GetCurrentAnimatorStateInfo(layerIndex).IsName (stateName);
        return result;
    }
    

    public void OnJumpEnter()
    {
        canAttack = false;

        pl.inputEnabled = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, jumphigh, 0);
    }
    public void OnRollEnter()
    {
        pl.inputEnabled = false;
        canAttack = false;
      
        thrustVec = new Vector3(0, rollSpeed, 0);
        lockPlanar = true;
    }
    public void OnRollExit()
    {
        canAttack = true;

        pl.inputEnabled = true;
        lockPlanar = false;
    }
    public void OnJumpExit()
    {
        canAttack = true;

        pl.inputEnabled = true;
        lockPlanar = false;
    }
    public void IsGround()
    {
        anima.SetBool("isGround", true);
    } 
    public void IsNotGround()
    {
        anima.SetBool("isGround", false);
    }
    public void OnGroundEnter()
    {
        pl.inputEnabled = true;
        lockPlanar = false;
        canAttack = true;
    }
    public void OnGroundExit()
    {
        col.material = frictionZero;
    }

    public void OnFallEnter()
    {
        pl.inputEnabled = false;
        lockPlanar = true;
    }
 
    public void OnJabEnter()
    {

        pl.inputEnabled = false;
        lockPlanar = true;
    }
    public void OnJabExit()
    {

        pl.inputEnabled = true;
        lockPlanar = false;
    }

    public void OnJabUpdate()
    {
        thrustVec = model.transform.forward * anima.GetFloat("jabVelocity");

    }

    public void OnAttack1hAEnter()
    {
        pl.inputEnabled = false;
        //lockPlanar = true;
        lerpTarget = 1.0f;
    }

    public void OnAttack1hAUpdate()
    {
        thrustVec = model.transform.forward * anima.GetFloat("attack1hAVelocity");

        float currentWeight = anima.GetLayerWeight(anima.GetLayerIndex("attack"));
        currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.9f);
        anima.SetLayerWeight(anima.GetLayerIndex("attack"), currentWeight);
    }


    public void OnAttackIdle()
    {
        pl.inputEnabled = true;
        //lockPlanar = false;
        //anima.SetLayerWeight(anima.GetLayerIndex("attack"), 0);
        lerpTarget = 0.10f;
    }
    public void OnAttackIdleUpdate()
    {

        float currentWeight = anima.GetLayerWeight(anima.GetLayerIndex("attack"));
        currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.9f);
        anima.SetLayerWeight(anima.GetLayerIndex("attack"), currentWeight);
    }

}
