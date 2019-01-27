using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HomeTakeover.Util;

public class AudioTester : MonoBehaviour
{

    // public SoundPool.SoundTypes type;
    [Range(0, 19)]
    public int soundIndex;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            SoundPool.Instance.PlaySound(soundIndex);
        }
    }
}
