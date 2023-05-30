using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerTextSpin : MonoBehaviour
{

    [SerializeField] private float spinSpeed = 250f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0, 0, -spinSpeed * Time.deltaTime);
    }
}
