using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectFloor : MonoBehaviour
{
    private PlayerControlManager playerCM;
    // Start is called before the first frame update
    void Start()
    {
        playerCM = GameObject.FindWithTag("Player").GetComponent<PlayerControlManager>();
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Floor")
            playerCM.isFloor = true;
            
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Floor")
            playerCM.isFloor = false;
    }
}
