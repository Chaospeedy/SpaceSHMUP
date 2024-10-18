using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero      S { get; private set;}

    // Start is called before the first frame update
    [Header("Inscribed")]
    public float        speed = 30;
    public float        rollMult = -45;
    public float        pitchMult = 30;
    public GameObject   projectilePrefab;
    public float        projectileSpeed = 40;
    public Weapon[]     weapons;

    [Header("Dynamic")]
    private float _shieldLevel = 1;
    //public float        shieldLevel = 1;

    [Tooltip( "This field holds a reference to the last triggering GameObject")]
    private GameObject lastTriggeringGo = null;

    public delegate void WeaponFireDelegate();

    public event WeaponFireDelegate fireEvent;

    void Awake(){
        if(S == null){
            S = this;
        }else{
            Debug.LogError("HeroAwake() - Attempt to assign second Hero.S!");
        }

        //fireEvent += TempFire;
    }


    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += hAxis * speed * Time.deltaTime;
        pos.y += vAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(vAxis*pitchMult, hAxis*rollMult, 0);

        //if(Input.GetKeyDown(KeyCode.Space)){
        //    TempFire();
        //}

        if(Input.GetAxis("Jump") == 1 && fireEvent != null){
            fireEvent();
        }
    }

    /* void TempFire(){
        GameObject projGO = Instantiate<GameObject>( projectilePrefab);
        projGO.transform.position = transform.position;
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        //rigidB.velocity = Vector3.up * projectileSpeed;

        ProjectileHero proj = projGO.GetComponent<ProjectileHero>();
        proj.type = eWeaponType.blaster;
        float tSpeed = Main.GET_WEAPON_DEFINITION(proj.type).velocity;
        rigidB.velocity = Vector3.up * tSpeed;
    } */

    void OnTriggerEnter(Collider other){
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        //Debug.Log("Shield trigger hit by: " + go.gameObject.name);

        if(go == lastTriggeringGo) return;
        lastTriggeringGo = go;
         Enemy enemy = go.GetComponent<Enemy>();
         PowerUp pUp = go.GetComponent<PowerUp>();
         if( enemy != null){
            shieldLevel--;
            Destroy(go);
         }else if(pUp != null){
            AbsordPowerUp(pUp);
         }else{
            Debug.LogWarning("Shield trigger hit by non-Enemy: " + go.name);
         }
    }

    public void AbsordPowerUp(PowerUp pUp){
        Debug.Log("Absorbed PowerUp: " + pUp.name);
        switch(pUp.type){
            
        }
        pUp.AbsorbedBy(this.gameObject);
    }

    public float shieldLevel{
        get { return ( _shieldLevel);}
        private set{
            _shieldLevel = Mathf.Min(value, 4);
            if (value < 0){
                Destroy(this.gameObject);
                Main.HERO_DIED();
            }
        }
    }
}
