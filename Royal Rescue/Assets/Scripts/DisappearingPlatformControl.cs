using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatformControl : MonoBehaviour
{
    [SerializeField] private List<DisappearingPlatform> platforms;
    [SerializeField] private float interval;

    private DisappearingPlatform previousPlatform, currentPlatform;
    private int index = 0;

    void OnEnable()
    {
        StartCoroutine(ActivatePlatforms());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator ActivatePlatforms()
    {
        while (gameObject.activeSelf)
        {
            if (index == platforms.Count)
            {
                for (int i = 0; i < platforms.Count; i++)
                    platforms[i].SetState(PlatformState.DISAPPEAR);
                
                index = 0;
                yield return new WaitForSeconds(interval);
                continue;
            }
            platforms[index].SetState(PlatformState.APPEAR);
            yield return new WaitForSeconds(interval);

            if (index > 0)
            {
                platforms[index - 1].SetState(PlatformState.DISAPPEAR);
                yield return new WaitForSeconds(interval / 3);
            }
            ++index;
        }
    }
}
