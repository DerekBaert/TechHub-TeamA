using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private void Awake()
    {
        if (LevelManager.instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void GameOver()
    {
        UIManager _ui = GetComponent<UIManager>();
        if (_ui != null)
        {
            // prefer an existing Stopwatch reference if available
            Stopwatch sw = _ui.stopwatch != null ? _ui.stopwatch : FindObjectOfType<Stopwatch>();

            string formatted = "Personal Best: 0:00.00s";
            if (sw != null)
            {
                formatted = sw.GetFormattedHighestTime("Personal Best: ");
            }

            _ui.ShowDeathPanelWithFormattedTime(formatted);
        }
    }
}
