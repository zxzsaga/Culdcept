using System.Collections.Generic;
using UnityEngine;

namespace Culdcept {
    public class Ground : MonoBehaviour {

        public List<Ground> adjacentGrounds;

        protected void Reset() {
            adjacentGrounds = new List<Ground>();
            SetAdjacentGrounds();
        }

        private void SetAdjacentGrounds() {
            Ground[] grounds = FindObjectsOfType<Ground>();
            for (int i = grounds.Length - 1; i >= 0; i--) {
                Ground ground = grounds[i];
                if (ground == this) {
                    continue;
                }
                if (adjacentGrounds.Contains(ground)) {
                    continue;
                }
                if (Vector3.Distance(transform.position, ground.transform.position) > 1.1f) {
                    continue;
                }
                adjacentGrounds.Add(ground);
            }
        }

        public List<Ground> NextGrounds(Ground previousGround) {
            if (previousGround == null) {
                return adjacentGrounds;
            }
            List<Ground> nextGrounds = new List<Ground>(adjacentGrounds);
            if (!nextGrounds.Remove(previousGround)) {
                Debug.Log("Previous ground is not adjacent with this ground.");
                return null;
            }
            return nextGrounds;
        }
    }
}
