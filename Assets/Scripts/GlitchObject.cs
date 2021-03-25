using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchObject : MonoBehaviour
{

    public GameManager gm; //referenced from gameplay instantiation method

    public void PointToActivateBadMode()
    {
        gm.ActivateBadMode();
    }

}
