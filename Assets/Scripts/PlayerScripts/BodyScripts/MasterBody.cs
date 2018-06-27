using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterBody : MonoBehaviour
{
    public bool keyboard = false; // voor testing purposes only

    public int playerPrefix; //This determines what player is using this body. Starts counting at 1 for it being more intuitive.
    public int playerID;

    public MasterHead head;
    public MasterArm rArm;
    public MasterArm lArm;
    public MasterLegs legs;

    public enum activeArm
    {
        left,
        right
    }
    public activeArm currentArm = activeArm.left;
    public float maxArmScale = 1.5f;
    public float minArmScale = 0f;
    bool switchbutton = false;

    public Color playerColor;
    public GameObject[] prefabArray;
    public List<GameObject> holder;
    private Transform[] transformArray;

    [SerializeField]
    private GameObject deathEffect;
    GameObject particleObject;

    //Creates a refrence for the playerstats. This is so when the player gets hit by a bullet, he can show his health.
    public playerStats playerStats;

    [SerializeField]
    protected float addedMass;      //added body Mass of the player. This will get calculated with each body part.

    //Players getting hit:
    public float basehealth = 10;  //How much health can the player have in total?
    public float health;           //How much health does the player have?
    public bool isAlive;             //Is the player currently dead?

    public bool invertControls; // invert controls (koala head)

    private CameraControl cameraControl;

    private Explosion explode;    

    public bool cdFunction = false;
    public bool canTakeDamage = true;
    public bool canAct = true;
    public bool canMove = true;

    //this object is spawned upon death
    private GameObject deathObject;
    // Use this for initialization

    //For playing sound effects:
    AudioSource audioSource;
    [SerializeField]
    AudioClip sndHit;
    [SerializeField]
    AudioClip weaponSwitch;
    [SerializeField]
    AudioClip deadSound;

    protected void Start()
    {
        audioSource = GetComponent<AudioSource>();

        isAlive = true;

        // Fetches CameraRigh script.
        if (GameObject.Find("CameraRig") != null)
        {
            cameraControl = GameObject.Find("CameraRig").GetComponent<CameraControl>();
            cameraControl.addPlayer(gameObject);
        }

        health = basehealth; // health is wat er later vanaf gaat als weapon damage doet

        holder = new List<GameObject>();
        transformArray = GetComponentsInChildren<Transform>();
        for (int i = 0; i < 5; i++)
        {
            holder.Add(Instantiate(prefabArray[i], transformArray[i].position + prefabArray[i].transform.position, Quaternion.identity));
            holder[i].transform.SetParent(gameObject.transform);
        }

        legs = holder[4].GetComponent<MasterLegs>();
        legs.Construct();
        //legs.controller = GetComponent<CharacterController>();
        head = holder[1].GetComponent<MasterHead>();
        lArm = holder[2].GetComponentInChildren<MasterArm>(); // get component in children is for balloon of doom item
        rArm = holder[3].GetComponentInChildren<MasterArm>(); // get component in children is for balloon of doom item

        lArm.transform.localScale = new Vector3(-maxArmScale, maxArmScale, maxArmScale);
        lArm.transform.localPosition = rArm.transform.localPosition - new Vector3(rArm.transform.localPosition.x * 2, 0, 0);
        rArm.transform.localScale = new Vector3(-minArmScale, minArmScale, minArmScale);

        legs.SetTotalMass(addedMass + lArm.AddedMass() + rArm.AddedMass() + head.AddedMass());

        //Changes the transforms of the guns so the left gun is on the left, -0.5, en the right gun is on the right, 0.5);
        Transform[] tempGunsObject = holder[2].GetComponentsInChildren<Transform>();

        lArm.bulletSpawn = tempGunsObject[2];

        Transform[] tempGunsObject2 = holder[3].GetComponentsInChildren<Transform>();

        rArm.bulletSpawn = tempGunsObject2[2];

        //TODO: When we want to add more health through the other body parts, we can use this calculation.
        //health = basehealth + rArm.healthMod + lArm.healthMod + legs.healthMod + head.healthMod;

        //Changes the color of the player based on the player number

        playerStats = GameManager.instance.GetComponent<playerStats>();


        playerColor = GameManager.instance.playerColors[playerID + 1]; //Set the color of the player to the right one. 1 = blue, 2 = red etc..

        lArm.color = this.playerColor;
        rArm.color = this.playerColor;
        //sets the color of the player
        setColor(playerColor);

    }

    public void OnLevelWasLoaded(int level)
    {
        StartCountDown();
    }

    public void StartCountDown()
    {
        //StartCoroutine(BeforeRounStart());
        //StartCoroutine(BeforeGoShows());
        
    }

    public void SetDeathObject()
    {
        deathObject = Instantiate(gameObject);
        Component[] components = deathObject.GetComponents(typeof(Component));

        deathObject.AddComponent<Rigidbody>();
        CapsuleCollider CC = deathObject.AddComponent<CapsuleCollider>();
        CC.radius = deathObject.GetComponent<CharacterController>().radius;
        CC.height = deathObject.GetComponent<CharacterController>().height;

        deathObject.AddComponent<AudioSource>();
        deathObject.GetComponent<AudioSource>().clip = deadSound;
        deathObject.GetComponent<AudioSource>().Play();
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] is MonoBehaviour || components[i] is CharacterController)
            {
                Destroy(components[i]);
            }
        }

        components = deathObject.GetComponentsInChildren(typeof(Component));
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] is MonoBehaviour)
            {
                Destroy(components[i]);
            }

        }
        deathObject.transform.position = transform.position;
        deathObject.transform.rotation = transform.rotation;
        deathObject.tag = "Untagged";
        deathObject.SetActive(true);

    }

    public void setColor(Color color)
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            if (r.material.color == Color.white)
            {
                r.material.color = color;
            }

        }

    }

    public void TakeDamage(GameObject attacker, float damage, float force = 0, Vector3 impactDirection = new Vector3())
    {

        playSound(sndHit);

            // impactDirection.y += 0.5f;
            if (attacker.Equals(gameObject))
            {
                return;
            }

            legs.addImpact(impactDirection.normalized, force);
        if (canTakeDamage)
        {
            // Instantiate the particle effect upon hit.
            particleObject = Instantiate(deathEffect);
            particleObject.transform.position = transform.position;
            particleObject.transform.rotation = Quaternion.LookRotation(impactDirection.normalized);

            // code for health / player dying
            this.health -= damage;
            playerStats.showHealth(gameObject, playerID);       // Shows the player health bar when he gets hit./
            GameManager.instance.scoreCounter.updateScore(attacker.GetComponent<MasterBody>().playerID, "hurt", (int)Mathf.Ceil(damage));
            if (this.health <= 0)
            {
                Debug.Log("health decreased.");
                GameManager.instance.scoreCounter.updateScore(attacker.GetComponent<MasterBody>().playerID, "kill");
                PlayerDies();
            }
        }
    }

    public void PlayerDies()
    {
        if (cdFunction == true)
        {
            gameObject.transform.position = GameManager.instance.positionHolder[playerID].transform.position;
            return;
        }
        if (isAlive)
        {
            isAlive = false;
            SetDeathObject();
        }
        Debug.Log("player dead.");
        cameraControl.removePlayer(name);
        GameManager.instance.updatePlayer(this);
        health = 0;
        gameObject.SetActive(false);
        //Destroy(gameObject);

    }


    // Update is called once per frame
    protected void Update()
    {

        //////////////////////////// KEYBOARD TESTING PURPOSES, KEEP IT PLEASE //////////////////////////////////////////////


        // ----------------------------------------------------------CONTROLS CODE--------------------------------------------------------
        //this ("prefix") allowes the keyboard to be used without breaking anything else that relies on playerprefix

     string prefix =  playerPrefix.ToString();
        if (keyboard)
        {
            prefix = "0";
        }

        lArm.PointArms(Input.GetAxis("P" + prefix + "_RightH"), Input.GetAxis("P" + prefix + "_RightV"));
        rArm.PointArms(Input.GetAxis("P" + prefix + "_RightH"), Input.GetAxis("P" + prefix + "_RightV"));
        if ((Input.GetButton("P" + prefix + "_LeftGun") || Input.GetButton("P" + prefix + "_RightGun")))
        {
            if (!switchbutton)
            {
                playSound(weaponSwitch);
                switchbutton = true;
                if (currentArm == activeArm.right)
                {
                    currentArm = activeArm.left;
                    lArm.transform.localScale = new Vector3(-maxArmScale, maxArmScale, maxArmScale);
                    rArm.transform.localScale = new Vector3(minArmScale, minArmScale, minArmScale);
                }
                else
                {
                    currentArm = activeArm.right;
                    rArm.transform.localScale = new Vector3(maxArmScale, maxArmScale, maxArmScale);
                    lArm.transform.localScale = new Vector3(-minArmScale, minArmScale, minArmScale);
                }
            }
        }
        else
        {
            switchbutton = false;
        }
        if (
                ((Input.GetAxis("P" + prefix + "_RightH") > 0.1f || Input.GetAxis("P" + prefix + "_RightH") < -0.1f) ||
                (Input.GetAxis("P" + prefix + "_RightV") > 0.1f || Input.GetAxis("P" + prefix + "_RightV") < -0.1f))
            && canAct)
        {
            if (currentArm == activeArm.left)
            {
                lArm.Fire();
            }
            else
            {
                rArm.Fire();
            }
            //Input.GetButton("P" + prefix + "_LeftGun")
        };
        if (canMove)
        {
            if (invertControls == false)
            {
                legs.Move(Input.GetAxis("P" + prefix + "_Horizontal"), Input.GetAxis("P" + prefix + "_Vertical"));
            }
            else if (invertControls)
            {
                //invertControls = true;
                legs.Move(-Input.GetAxis("P" + prefix + "_Horizontal"), -Input.GetAxis("P" + prefix + "_Vertical"));
            }
        }
        legs.updateImpact();
        //CheckCollision();

    }
    bool disableGUI = false;
    //public IEnumerator BeforeRounStart(float roundCDreduction = 3)
    //{
    //    roundCD = roundCDreduction;
    //    // in de timer, disable functions (shoot and/or get ranged weapon script) / playerdies (deadzone)
    //    while (roundCD > -1)
    //    {
    //        Debug.Log("Countdown: " + roundCD);
    //        yield return new WaitForSeconds(1.0f);
    //        roundCD--;

    //        cdFunction = true;
    //        canTakeDamage = false;
    //        canAct = false;
    //        disableGUI = false;
    //        // showtext (UI) (initialize somewhere else in script)
    //    }
    //    // activates shooting, takedamage & hides UI
    //    if (roundCD == -1)
    //    {
    //        cdFunction = false;
    //        canTakeDamage = true;
    //        canAct = true;
    //        disableGUI = true;
    //    }
    //}

    //bool showGo = false;
    //public IEnumerator BeforeGoShows(float roundCDreduction = 3)
    //{
    //    roundCD = roundCDreduction;
    //    // in de timer, disable functions (shoot and/or get ranged weapon script) / playerdies (deadzone)
    //    while (roundCD > 0)
    //    {
    //        yield return new WaitForSeconds(3.0f);
    //    }
    //}
    //public Font f;
    //void OnGUI()
    //{
    //    if (!f)
    //    {
    //        Debug.LogError("No font found, assign one in the inspector.");
    //    }
    //    if (roundCD > -1)
    //    {
    //        if (roundCD > 0) // if it's counting down 3,2,1
    //        {
    //            GUI.skin.font = f;
    //            GUI.skin.label.fontSize = 70;
    //            GUI.color = Color.white;
    //            GUI.Label(new Rect(620, 45, 300, 300), "" + roundCD);
    //        }
    //        else // shows the go
    //        {
    //            GUI.skin.font = f;
    //            GUI.skin.label.fontSize = 90;
    //            GUI.color = Color.white;
    //            GUI.Label(new Rect(525, 45, 300, 300), "GO");
    //            // enabled = false;
    //        }
    //    }
    //}

    public void playSound(AudioClip soundEffect)
    {
        audioSource.clip = soundEffect;
        audioSource.Play();
    }
}