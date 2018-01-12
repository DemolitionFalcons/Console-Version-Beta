using DemolitionFalcons.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemolitionFalcons.App.Miscellaneous.SpecialSquares
{
    public class MysterySquareAction
    {
        public MysterySquareAction()
        {
            MoveForward = false;
            GoBack = false;
            DemolitionFalcons = false;
        }

        #region ProvideInfoAboutTheResultOfTheGame
        public bool MoveForward { get; set; }
        public int MoveForwardWith { get; set; }

        public bool GoBack { get; set; }
        public int GoBackWith { get; set; }

        public bool DemolitionFalcons { get; set; }
        #endregion

        public void PlayMiniGame()
        {
        }


    }
}
