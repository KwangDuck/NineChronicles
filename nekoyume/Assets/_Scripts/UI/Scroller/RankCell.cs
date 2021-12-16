using Nekoyume.State;
using Nekoyume.UI.Model;
using UnityEngine;

namespace Nekoyume.UI.Scroller
{
    public class RankCell : RectCell<
        RankingModel,
        RankScroll.ContextModel>
    {
        [SerializeField]
        private RankCellPanel rankCell = null;

        [SerializeField]
        private RankCellPanel myInfoRankCell = null;

        public override void UpdateContent(RankingModel viewModel)
        {
            var isMyInfo = true;
            var cell = isMyInfo ? myInfoRankCell : rankCell;
            rankCell.gameObject.SetActive(!isMyInfo);
            myInfoRankCell.gameObject.SetActive(isMyInfo);
            cell.SetData(viewModel);
        }
    }
}
