namespace QQ
{
    public class SkillBox : SkillSystem
    {
        protected override void OnFocus()
        {
            base.OnFocus();

            ExplodeDamage();
        }
    }
}
