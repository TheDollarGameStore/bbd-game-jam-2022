using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transitioner : MonoBehaviour
{
    // Start is called before the first frame update
    public static Transitioner instance = null;
    [SerializeField] private GameObject transition;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TransitionToScene(int sceneNumber)
    {
        if (GameObject.FindGameObjectsWithTag("Transition").Length == 0)
        {
            Transition component = Instantiate(transition, Vector3.zero, Quaternion.identity).GetComponentInChildren<Transition>();

            component.fadeIn = true;
            component.targetScene = sceneNumber;
        }
    }
}
