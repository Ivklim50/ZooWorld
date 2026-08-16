using TMPro;
using UnityEngine;
using VContainer;
using ZooWorld.Stats;

namespace ZooWorld.UI
{
    public sealed class StatsPresenter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _deadPreyText;
        [SerializeField] private TMP_Text _deadPredatorsText;

        private GameStats _stats;

        [Inject]
        public void Construct(GameStats stats)
        {
            _stats = stats;
            _stats.Changed += Redraw;
            Redraw();
        }

        private void OnDestroy()
        {
            if (_stats != null) _stats.Changed -= Redraw;
        }

        private void Redraw()
        {
            _deadPreyText.text = $"Dead prey: {_stats.DeadPrey}";
            _deadPredatorsText.text = $"Dead predators: {_stats.DeadPredators}";
        }
    }
}
