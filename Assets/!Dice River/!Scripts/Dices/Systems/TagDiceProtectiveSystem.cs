using BitterECS.Core;

public class TagDiceProtectiveSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;
    private EcsEvent _ecsEventProtect =
      new EcsEvent<DicePresenter>()
      .SubscribeWhereEntity<IsTargetingEvent>(e =>
        EcsConditions.Has<TagProtectiveDice>(e), OnProtectingRole);

    private static void OnProtectingRole(EcsEntity entity)
    {
        //TODO: Перекидывание урона
    }
}
