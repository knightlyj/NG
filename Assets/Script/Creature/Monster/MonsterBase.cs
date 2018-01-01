using UnityEngine;
using System.Collections;

public class MonsterBase : Creature {

    void OnOptionChosen(int idx)
    {
        Helper.HideDialog();
    }

    public virtual void TalkWith(Player player)
    {
        Helper.ShowDialog("hellow world", new string[] { "bye", "thank u" }, this.OnOptionChosen);
    }
}
