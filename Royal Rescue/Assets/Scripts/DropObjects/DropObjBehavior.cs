using System.Collections;
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
    [SerializeField] float detectGroundRayDistance;
    [SerializeField] bool destroyObj = false;
    [SerializeField] LayerMask[] detectLayers;
    int detectLayer = 0;
    Ray warningZoneSpawnRay;
    RaycastHit warningZoneSpawnHit;
    GameObject deactiveDangerZoneObj;
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
        warningZoneSpawnRay = new Ray(transform.position, Vector3.down);
        Physics.Raycast(warningZoneSpawnRay, out warningZoneSpawnHit, 50, detectLayer);
        deactiveDangerZoneObj = pulling.SpawnObject(dangerZoneObj.tag, warningZoneSpawnHit.point);
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
            //�÷��̾� ������ ���� �����ۼ� ��


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

    public string GetTag()
    {
        return detailTag;
    }
    public bool CompareToTag(string detailTag)
    {
        return this.detailTag == detailTag;
    }
}
