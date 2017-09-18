using UnityEngine;

namespace Culdcept {
    public class Character : MonoBehaviour {

        public int Gold = 500;

        public Ground currentGround;

        public int GetSteps() {
            return Random.Range(1, 7);
        }

        public void MoveBySteps(int step) {
            for (int i = 0; i < step; i++) {
                MoveToNextGround();
            }
        }

        public void MoveToNextGround() {

        }
    }
}
