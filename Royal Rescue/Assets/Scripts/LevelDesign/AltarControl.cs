using UnityEngine;

public class AltarControl : MonoBehaviour
{
    public static int redGem, whiteGem, greenGem = 0;
    private const int TOTALALTARS = 3;

    [SerializeField] private AltarPortal altarPortal;

    private int activatedAltars = 0;

    public bool CanActivateAltar(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.RUBY:
                return redGem > 0;

            case ResourceType.DIAMOND:
                return whiteGem > 0;                

            case ResourceType.JADE:
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
}

