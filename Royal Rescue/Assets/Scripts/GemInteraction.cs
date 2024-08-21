using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemInteraction : MonoBehaviour
{
    [SerializeField] private GameObject gemEffect;
    [SerializeField] private GemType gemType;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("보석 획득!");
            gameObject.SetActive(false);

            GameObject effect = Instantiate(gemEffect, transform.parent);
            effect.transform.position = other.transform.position;
            effect.GetComponent<ParticleSystem>().Play();
            Destroy(effect, 0.9f);

            // 임시 코드 ... 추후에 아이템을 늘려서 개수 확인하는 식으로 변경 ////
            switch (gemType)
            {
                case GemType.RED:
                    AltarControl.redGem++;
                    break;

                case GemType.WHITE:
                    AltarControl.whiteGem++;
                    break;

                case GemType.GREEN:
                    AltarControl.greenGem++;
                    break;

            }
            /////////////////////////////////////////////////////////////////
        }
    }
}
public enum GemType { RED, WHITE, GREEN }; // 추후에 enum 없앰
