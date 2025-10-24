using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryEvent", menuName = "StoryData/StoryEvent")]
public class StoryEvent : ScriptableObject {

    public Sprite eventPicture;
    public string title = "Default Story Event";
    [TextArea(2, 10)]
    public string textContent = "Hello World";

}
