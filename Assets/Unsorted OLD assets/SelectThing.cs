using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectThing : MonoBehaviour
{
    public InputField ifBox;
    public TMP_InputField ifBox2;

    int boxSelected = 0;

    // Start is called before the first frame update
    void Start()
    {
        ifBox2.Select();
    }

    // Update is called once per frame
    void Update()
    {
        if(boxSelected == 0) {
            if(Input.GetKeyDown(KeyCode.Alpha0)) {
                boxSelected = 1;
                ifBox.Select();
            }
        }
    }
}
