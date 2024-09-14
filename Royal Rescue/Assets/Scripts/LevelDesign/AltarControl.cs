using UnityEngine;

public class AltarControl : MonoBehaviour
{
    public static int redGem, whiteGem, greenGem = 0;
    private const int TOTALALTARS = 3;

    [SerializeField] private AltarPortal altarPortal;

    private int activatedAltars = 0;

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
        altarPortal.TryOpenStagePortal(activatedAltars == TOTALALTARS);
    }

    static public void ResetAltar()
    {
        redGem = whiteGem = greenGem = 0;
    }
}

