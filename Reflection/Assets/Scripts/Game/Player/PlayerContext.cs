using UniRx;
using UnityEngine;

namespace Reflection.Game.Player
{

    internal class PlayerContext : MonoBehaviour
    {

        internal static ReactiveProperty<MoveType> MoveType = new ReactiveProperty<MoveType>();

    }

}