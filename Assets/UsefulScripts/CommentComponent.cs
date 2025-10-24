using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class can be attached to a game object to allow for the use of comments in the editor
public class CommentComponent : MonoBehaviour
{
    // Attributes like these are a special type of marker that will 
    // appear in the Editor. They are not a part of standard C# coding
    // https://docs.unity3d.com/Manual/Attributes.html

    // Header Attribute
    //https://docs.unity3d.com/ScriptReference/HeaderAttribute.html
    [Header("Comment Here:")]

    // Text Area Attribute
    // https://docs.unity3d.com/ScriptReference/TextAreaAttribute.html
    // The 3 means show a min of 3 lines, the 20 means a max of 10 before it becomes a scrollbar. 
    // These are provided as overloaded parameters in the docs but not really seen in examples provided
    [TextAreaAttribute (3, 20)]
    public string AddContextToTheGameObject;
}
