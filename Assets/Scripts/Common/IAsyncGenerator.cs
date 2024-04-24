using Cysharp.Threading.Tasks;

namespace MapGeneration
{
    public interface IAsyncGenerator
    {
        public UniTask Generate();
    }
}