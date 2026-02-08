
using BitterECS.Core;
using Cysharp.Threading.Tasks;

public interface IHandSucceedRemove : IEcsSystem
{
    public UniTask ResultSucceedRemove();
}

public interface IHandFailRemove : IEcsSystem
{
    public UniTask ResultFailRemove();
}

public interface IHandSucceedAdd : IEcsSystem
{
    public UniTask ResultSucceedAdd();
}

public interface IHandFailAdd : IEcsSystem
{
    public UniTask ResultFailAdd();
}

public interface IHandSucceedExtraction : IEcsSystem
{
    public UniTask ResultSucceedExtraction();
}

public interface IHandFailExtraction : IEcsSystem
{
    public UniTask ResultFailExtraction();
}
