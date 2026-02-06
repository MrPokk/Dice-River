using BitterECS.Core;

public class DiceTagProtectiveSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;
    private EcsEvent _ecsEventProtect =
      new EcsEvent<DicePresenter>()
      .SubscribeWhereEntity<RollComponent>(e =>
        EcsConditions.Has<TagProtectiveDice>(e), OnProtectingRole);

    private static void OnProtectingRole(EcsEntity entity)
    {
        //TODO: Перекидывание урона
    }
}
