namespace QQ
{
    public class SkillItem : SkillSystem
    {
        protected override void OnFocus()
        {
            base.OnFocus();

            // 아이템 효과 적용
            AddStat(data.mainOptionType);

            if(data.mainOptionType != data.subOptionType)
            {
                AddStat(data.subOptionType);
            }
        }
    }
}
