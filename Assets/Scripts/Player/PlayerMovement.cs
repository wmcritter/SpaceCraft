using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public bool Disabled = false;
    public float speed = 2.0f;
    //public AudioSource thrusterAudio;
    //public AudioSource alarmAudio;
    //public Text warningCloseText;
    //public Text warningFarText;
    public float thrusterEnergyUse = 0.1f;
    public float verticalThrusterRatio = 2.0f;
    //PlayerHealth playerHealth;

    bool isDead = false;

    // Use this for initialization
    void Start()
    {
        /*playerHealth = gameObject.GetComponent<PlayerHealth>();
        if (GameController.currentController.SavedGame != null)
        {
            if (GameController.currentController.SavedGame.playerPosition != Vector3.zero)
                gameObject.transform.position = GameController.currentController.SavedGame.playerPosition;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead || Disabled) return;

        //if (MyWorld.currentWorld != null &&
        //    MyWorld.currentWorld.IsPlayerMovementDisabled)
        //    return;

        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        var h = Input.GetAxis("Horizontal"); //left and right
        var v = Input.GetAxis("Vertical"); //forward > 0, back < 0
        bool boosters = Input.GetAxis("Jump") == 1;// GetKey(KeyCode.Space);

        //up and down
        float y = 0.0f;
        //if (Input.GetKey(KeyCode.LeftShift))
        if (Input.GetAxis("ThrustVerticalUp") == 1)
            y += (speed / verticalThrusterRatio) * Time.deltaTime;
        //else if (Input.GetKey(KeyCode.LeftControl))
        else if (Input.GetAxis("ThrustVerticalDown") == 1)
            y += ((speed / verticalThrusterRatio) * -1) * Time.deltaTime;

        //apply boosters
        float boosterScale = (boosters && v > 0) ? 3 : 1;

        //move
        transform.Translate(
            h * speed * Time.deltaTime,
            y,
            v * speed * boosterScale * Time.deltaTime);

        //Debug.Log (string.Format("h:{0} v:{1}", h, v));
        if (h != 0 || v != 0 || y != 0)//no movement
        {
            //playerHealth.UseEnergy((float)(thrusterEnergyUse * Time.deltaTime));
            //if (!thrusterAudio.isPlaying)
            //    thrusterAudio.Play();
        }
        else //moving
        {
            //if (thrusterAudio.isPlaying)
                //thrusterAudio.Stop();
        }

        /*SunProximity prox = MyWorld.currentWorld.GetSunProximity(transform.position);
        switch (prox)
        {
            case SunProximity.LethalClose: Die(); break;
            case SunProximity.LethalFar: Die(); break;
            case SunProximity.WarningClose:
                if (!alarmAudio.isPlaying)
                {
                    alarmAudio.Play();
                    warningCloseText.gameObject.SetActive(true);
                }
                break;
            case SunProximity.WarningFar:
                if (!alarmAudio.isPlaying)
                {
                    alarmAudio.Play();
                    warningFarText.gameObject.SetActive(true);
                }
                break;
            case SunProximity.Safe:
                if (alarmAudio.isPlaying)
                {
                    alarmAudio.Stop();
                    warningCloseText.gameObject.SetActive(false);
                    warningFarText.gameObject.SetActive(false);
                }
                break;
        }*/
    }

    void Die()
    {
        isDead = true;
    }

    void FixedUpdate()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.tag == Tags.item)
        {
            DroppedItem item = other.gameObject.GetComponent<DroppedItem>();

            if (item != null)
            {
                PlayerIO pio = gameObject.GetComponent<PlayerIO>();
                if (pio.AddToInventory(item.Item))
                {
                    if (other.gameObject != null)
                        Destroy(other.gameObject);

                }
            }
        }*/
    }
}
