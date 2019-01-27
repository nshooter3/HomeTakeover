using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HomeTakeover.Util;

public class AudioTester : MonoBehaviour
{

    public SoundPool.SoundTypes type;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            SoundPool.Instance.PlaySound(type);
        }
    }
}
