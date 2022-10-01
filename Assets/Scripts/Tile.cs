using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [HideInInspector] public Lumin lumin;
    [HideInInspector] public int y;
    [HideInInspector] public int x;

    private void Update()
    {
        if (lumin != null)
        {
            lumin.transform.position = Vector3.Lerp(lumin.transform.position, transform.position, 10f * Time.deltaTime);
        }
    }
}
