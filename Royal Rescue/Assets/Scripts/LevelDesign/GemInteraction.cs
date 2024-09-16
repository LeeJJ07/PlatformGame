using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemInteraction : MonoBehaviour
{
    [SerializeField] private GemType gemType;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (gemType)
            {
                case GemType.RUBY:
                    AltarControl.redGem++;
                    break;

                case GemType.DIAMOND:
                    AltarControl.whiteGem++;
                    break;

                case GemType.JADE:
                    AltarControl.greenGem++;
                    break;
            }
        }
    }
}
public enum GemType { RUBY, DIAMOND, JADE }; // 추후에 enum 없앰
