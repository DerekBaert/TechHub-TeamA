using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ ] Custom Editor Class
public class ReadMe : MonoBehaviour
{
    [Header("Instructions")]
    [TextArea(4,100)]
    public string usageInstructions;

    [Header("Working Notes")]
    [TextArea(4, 100)]
    public string notes;

}
