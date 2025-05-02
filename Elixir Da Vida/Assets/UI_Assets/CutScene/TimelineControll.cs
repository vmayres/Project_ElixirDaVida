using UnityEngine;
using UnityEngine.Playables;


public class TimelineControll : MonoBehaviour
{
public PlayableDirector director;
    public GameObject dialogBox;

    void Start()
    {
        dialogBox.SetActive(false);
    }

    public void ShowDialog()
    {
        director.Pause();
        dialogBox.SetActive(true);
    }

    public void ContinueTimeline()
    {
        dialogBox.SetActive(false);
        director.Resume();
    }
}
