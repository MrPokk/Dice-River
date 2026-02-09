
using BitterECS.Core;
using Cysharp.Threading.Tasks;

public interface IHandSucceedRemove : IEcsAutoImplement
{
    public UniTask ResultSucceedRemove(HandControllerDice hand);
}

public interface IHandFailRemove : IEcsAutoImplement
{
    public UniTask ResultFailRemove(HandControllerDice hand);
}

public interface IHandSucceedAdd : IEcsAutoImplement
{
    public UniTask ResultSucceedAdd(HandControllerDice hand);
}

public interface IHandFailAdd : IEcsAutoImplement
{
    public UniTask ResultFailAdd(HandControllerDice hand);
}

public interface IHandSucceedExtraction : IEcsAutoImplement
{
    public UniTask ResultSucceedExtraction(HandControllerDice hand);
}

public interface IHandFailExtraction : IEcsAutoImplement
{
    public UniTask ResultFailExtraction(HandControllerDice hand);
}

public interface IHandResultInExtractEnded : IEcsAutoImplement
{
    public UniTask ResultInExtractEnded(HandControllerDice hand);
}

public interface IHandResultInRemoveEnded : IEcsAutoImplement
{
    public UniTask ResultInRemoveEnded(HandControllerDice hand);
}
