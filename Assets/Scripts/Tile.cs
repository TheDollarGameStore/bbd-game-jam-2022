using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [HideInInspector] public Lumin lumin;
    [HideInInspector] public int y;
    [HideInInspector] public int x;
    [SerializeField] public List<Connector> connectors;

    private bool isDeployed;

    private void Start()
    {
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (lumin != null)
        {
            lumin.transform.position = Vector3.Lerp(lumin.transform.position, transform.position, 10f * Time.deltaTime);
        }

        transform.localScale = Vector3.Lerp(transform.localScale, isDeployed ? Vector3.one : Vector3.zero, 10f * Time.deltaTime);
    }

    public void UpdateConnectors()
    {
        foreach(Connector con in connectors)
        {
            con.CheckDiodeMatch();
        }
    }

    private void Deploy()
    {
        isDeployed = true;
    }
}
