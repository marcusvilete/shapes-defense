using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestruction : MonoBehaviour
{

    public float destroyAfterSeconds = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, destroyAfterSeconds);
    }
}
