using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    [SerializeField] public List<Tile> tiles;

    [SerializeField] private ParticleSystemForceField field;

    void Start()
    {
        field.enabled = false;
    }

    public bool CheckDiodeMatch()
    {
        if (tiles[0].lumin != null && tiles[1].lumin != null && tiles[0].lumin.color == tiles[1].lumin.color)
        {
            this.field.enabled = true;
            return true;
        }
        else
        {
            this.field.enabled = false;
        }
        return false;

    }
}
