using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemButtonHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private Sprite gemSprite;

    public void SetGem(Sprite gem)
    {
        gemSprite = gem;
    }

    public Sprite GetGem()
    {
        return gemSprite;
    }
}
