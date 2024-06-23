using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class CharacterStatsView : MonoBehaviour
{
    [SerializeField] private Slider _hp;
    [Inject] private Player _player;

    private void Update()
    {
        int maxHp = _player.stats.maxHp;
        int hp = _player.stats.Hp;

        _hp.value = (float)hp / maxHp;
    }
}
