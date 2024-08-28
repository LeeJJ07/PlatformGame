using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DropObjBehavior : MonoBehaviour,ITag
{
    [Header("Detail Tag"), SerializeField]
    string detailTag="";
    //PlayerController playerController;
    PullingDirector pulling;
    [SerializeField] GameObject dangerZoneObj;
    [SerializeField] float delayTime = 0;
    [SerializeField] float dropSpeed = 1;
    [SerializeField] float damage = 1;
    [SerializeField] bool destroyObj = false;
    [SerializeField] LayerMask[] detectLayers;
    int detectLayer = 0;
    Ray warningZoneSpawnRay;
    RaycastHit warningZoneSpawnHit;
    Ray detectGroundRay;
    RaycastHit detectGroundHit;
    GameObject deactiveWarningZoneObj;
    Rigidbody rigid;
    bool isEndDelay = false;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        
        pulling = GameObject.FindWithTag("Director").GetComponent<PullingDirector>();
        foreach(LayerMask layer in detectLayers)
        {
            detectLayer |= layer;
        }
       
    }
    void OnEnable()
    {
        if (rigid != null)
        {
            rigid.isKinematic = true;
        }
        
        StartCoroutine(DropCoroutine());
        
        
    }
    void Update()
    {
        detectGroundRay = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(detectGroundRay.origin, detectGroundRay.direction, Color.green);
        Physics.Raycast(detectGroundRay, out detectGroundHit, 1f, detectLayer);
        if (detectGroundHit.collider != null)
        {
            if (detectGroundHit.collider.tag.Equals("Player"))
            {
                if (destroyObj)
                {
                    this.gameObject.SetActive(false);
                }
                if (rigid != null)
                {
                    rigid.isKinematic = false;
                }
                if (deactiveWarningZoneObj != null)
                    deactiveWarningZoneObj.SetActive(false);
                Debug.Log("Player");
                isEndDelay = false;
                //플레이어 데미지 전달 로직작성 블럭


            }
            if (detectGroundHit.collider.tag.Equals("Floor"))
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
                if(deactiveWarningZoneObj!=null)
                    deactiveWarningZoneObj.SetActive(false);
                Debug.Log("Floor");
                isEndDelay = false;
            }
        }
        Debug.Log($"isEndDelay: {isEndDelay}");
        if (isEndDelay)
            transform.position += Vector3.down * dropSpeed * Time.deltaTime;
        
    }
    IEnumerator DropCoroutine()
    {
        Debug.Log("Test");
        yield return new WaitForSeconds(delayTime);
        warningZoneSpawnRay = new Ray(transform.position, Vector3.down);
        Physics.Raycast(warningZoneSpawnRay, out warningZoneSpawnHit, 50, detectLayer);
        deactiveWarningZoneObj = pulling.SpawnObject(dangerZoneObj.tag, warningZoneSpawnHit.point);
        isEndDelay = true;
    }
   

    public string GetTag()
    {
        return detailTag;
    }
    public bool CompareToTag(string detailTag)
    {
        return this.detailTag == detailTag;
    }
}
