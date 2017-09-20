using System.Collections.Generic;
using UnityEngine;

namespace Culdcept {
    public class GameManager : MonoBehaviour {

        public SpecialGround baseGround;
        public List<SpecialGround> stations;

        public static GameManager Instance;

        private void Awake() {
            Instance = this;
        }

        private void NextTurn() {
        
        }
    }
}
