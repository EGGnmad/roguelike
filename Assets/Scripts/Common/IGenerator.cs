using Cysharp.Threading.Tasks;

namespace MapGeneration
{
    public interface IGenerator
    {
        public UniTask Generate();
    }
}