using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarControl : MonoBehaviour
{
    public static int redGem, whiteGem, greenGem = 0;

    private int activatedAltars = 0;
    private const int TOTALALTARS = 3;

    public bool CanActivateAltar(GemType gemType)
    {
        switch (gemType)
        {
            case GemType.RED:
                return redGem > 0;

            case GemType.WHITE:
                return whiteGem > 0;                

            case GemType.GREEN:
                return greenGem > 0;
            
            default:
                return false;
        }
    }
    public void ActivateAltar()
    {
        ++activatedAltars;
        TryOpenStagePortal();
    }

    private void TryOpenStagePortal()
    {
        if (activatedAltars == TOTALALTARS)
            Debug.Log("다음 스테이지로의 포탈이 열렸습니다.");
    }
}

