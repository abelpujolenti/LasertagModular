using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CountdownButton : MonoBehaviour
{
    public Button button;
    public Animator backAnimator;
    public SlicedFilledImage slicedFilledImage;

    public UnityEvent onInteracted;
    public UnityEvent onFinishedCoolDown;

    public float coolDownDuration = 1;

    private float coolDownCurrentTime = 0;
    private bool coolDownActive = false;

    public void buttonInteracted()
    {
        button.interactable = false;
        coolDownActive = true;
        onInteracted.Invoke();
        backAnimator.SetTrigger("Pressed");
    }

    private void Update()
    {
        if (coolDownActive )
        {
            coolDownCurrentTime += Time.deltaTime;
            slicedFilledImage.fillAmount = coolDownCurrentTime/ coolDownDuration;

            if (coolDownCurrentTime >= coolDownDuration)
            {
                coolDownCurrentTime = 0;
                button.interactable = true;
                coolDownActive = false;
                onFinishedCoolDown.Invoke();
            }
        }
    }
}