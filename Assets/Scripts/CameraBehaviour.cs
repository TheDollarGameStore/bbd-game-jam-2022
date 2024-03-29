using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Vector3 defaultPos;
    private float shakeIntensity;

    private float defaultSize;

    private Camera cameraComponent;

    // Start is called before the first frame update
    void Start()
    {
        cameraComponent = GetComponent<Camera>();
        defaultSize = cameraComponent.orthographicSize;
        cameraComponent.orthographicSize *= 0.5f;
        defaultPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        cameraComponent.orthographicSize = Mathf.Lerp(cameraComponent.orthographicSize, defaultSize, 3f * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, defaultPos, 10f * Time.deltaTime);

        shakeIntensity -= 30f * Time.deltaTime;

        if (shakeIntensity <= 0f)
        {
            shakeIntensity = 0f;
        }
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)new Vector2(Random.Range(-shakeIntensity, shakeIntensity), Random.Range(-shakeIntensity, shakeIntensity));
    }

    public void Nudge()
    {
        transform.position += (Vector3)Vector2.down * 6f;
    }

    public void Shake(float intensity)
    {
        shakeIntensity = intensity;
    }
}
