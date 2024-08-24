using System.Collections;
using UnityEngine;

public class DropObjBehavior : MonoBehaviour
{
    PlayerController playerController;
    PullingDirector pulling;
    [SerializeField] GameObject dangerZoneObj;
    [SerializeField] float delayTime = 0;
    [SerializeField] float dropSpeed = 1;
    [SerializeField] float damage = 1;
    [SerializeField] float detectGroundRayDistance;
    [SerializeField] bool destroyObj = false;
    [SerializeField] LayerMask[] detectLayers;
    Ray dangerZoneSpawnRay;
    RaycastHit dangerZoneSpawnHit;
    GameObject deactiveDangerZoneObj;
    Rigidbody rigid;
    bool isEndDelay = false;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        pulling = GameObject.FindWithTag("Director").GetComponent<PullingDirector>();
    }
    private void OnEnable()
    {
        if (rigid != null)
        {
            rigid.isKinematic = true;
        }
        StartCoroutine("DropCoroutine");
    }

    void Update()
    {
        if(isEndDelay)
            transform.position += Vector3.down * dropSpeed * Time.deltaTime;
    }
    IEnumerator DropCoroutine()
    {
        yield return new WaitForSeconds(delayTime);
        dangerZoneSpawnRay = new Ray(transform.position, Vector3.down);
        Physics.Raycast(dangerZoneSpawnRay, out dangerZoneSpawnHit, 30, LayerMask.GetMask("Ground"));
        deactiveDangerZoneObj = pulling.SpawnObject(dangerZoneObj.tag, dangerZoneSpawnHit.point);
        yield return new WaitForSeconds(delayTime);
        isEndDelay = true;
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            if (destroyObj)
            {
                this.gameObject.SetActive(false);
            }
            if (rigid != null)
            {
                rigid.isKinematic = false;
            }
            deactiveDangerZoneObj.SetActive(false);
            isEndDelay = false;
            //플레이어 데미지 전달 로직작성 블럭


        }
        if (other.tag.Equals("Ground"))
        {
            Debug.Log("collision");
            if (destroyObj)
            {
                this.gameObject.SetActive(false);
            }
            if (rigid != null)
            {
                rigid.isKinematic = false;
            } 
            
            deactiveDangerZoneObj.SetActive(false);
            isEndDelay = false;
        }
    }
}
