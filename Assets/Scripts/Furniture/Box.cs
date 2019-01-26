namespace HomeTakeover.Furniture
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Box : Furniture
    {
        public override void OnUse()
        {
            Debug.Log("BOX ATTACK!");
        }
    }
}
