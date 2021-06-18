using UniRx;
using UnityEngine;

namespace Reflection.Game.Player
{

    internal class PlayerContext
    {

        internal static ReactiveProperty<MoveType> MoveType = new ReactiveProperty<MoveType>();
        internal static Subject<float> OnStep = new Subject<float>();

    }

}