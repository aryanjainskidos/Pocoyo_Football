using UnityEngine;

public class SettingAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool isOn =  false;


    public void OnClick()
    {
        Debug.Log("Click on Onclick function");
        isOn = !isOn;
        if (isOn)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }


    void Show()
    {
        Debug.Log("click on show function");
        animator.Play("SettingOn");
    }


    void Hide()
    {   
        Debug.Log("click on hide function");
        animator.Play("SettingOff");
    }
}
