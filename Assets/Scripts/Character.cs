using System.Collections.Generic;
using UnityEngine;

namespace Culdcept {
    public class Character : MonoBehaviour {

        public int Gold = 500;

        [SerializeField, NotEditableInInspector] private Ground _currentGround;
        public Ground currentGround {
            get { return _currentGround; }
            set {
                previousGround = _currentGround;
                _currentGround = value;
                transform.position = _currentGround.transform.position + Vector3.up;
            }
        }
        public Ground previousGround;

        private void Start() {
            currentGround = GameManager.Instance.baseGround;
        }

        public int GetSteps() {
            return Random.Range(1, 7);
        }

        public void MoveBySteps(int step) {
            for (int i = 0; i < step; i++) {
                MoveToNextGround();
            }
        }

        public void MoveToNextGround() {
            List<Ground> nextGrounds = currentGround.NextGrounds(previousGround);
            currentGround = nextGrounds[0];
        }
    }
}
