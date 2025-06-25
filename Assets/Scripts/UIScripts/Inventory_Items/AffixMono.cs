using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AffixMono : MonoBehaviour
{
    public TMP_Text AffixName;
    public TMP_Text AffixNum;
    public Slider bar;
    public void Setup(Affix affix, Rarity rarity)
    {
        AffixRangeTable.Ranges.TryGetValue(rarity, out var range);
        AffixRangeTable.StatsMinAndMaxbyType.TryGetValue(affix.targetStat, out var rangeRatio);
        float min = rangeRatio.RangeLength * range.min;
        float max = rangeRatio.RangeLength * range.max;
        float barValue = (affix.value - min) / (max - min);
        AffixName.text = affix.targetStat.ToString();
        AffixNum.text = affix.value.ToString("P1");
        bar.value = barValue;
    }
}