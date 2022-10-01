using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Vector3 defaultPos;

    // Start is called before the first frame update
    void Start()
    {
        defaultPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, defaultPos, 10f * Time.deltaTime);
    }

    public void Nudge()
    {
        transform.position += (Vector3)Vector2.down * 6f;
    }
}
