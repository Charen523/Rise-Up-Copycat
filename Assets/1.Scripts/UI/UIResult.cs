using UnityEngine;

public class UIResult : UIBase
{
    [SerializeField] private GameObject slotScorePrefab;
    [SerializeField] private Transform slotParent;

    public override void Opened(object[] param)
    {
        int score = UIManager.Get<UIScore>().GetScore();
        UIManager.Hide<UIScore>();

        SlotScore slot = Instantiate(slotScorePrefab, slotParent).GetComponent<SlotScore>();
        slot.SetScore(score);

        SaveManager.Instance.SaveScore(score);
    }
}